using Microsoft.IdentityModel.Tokens;
using RedSocial.Modelos;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RedSocial.Servicios
{
    public interface ITokenService
    {
        string CreateToken(Usuarios usuario);
        int ObtencionIdUsuario(ClaimsIdentity identity);
    }

    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _ssKey;
        public string CreateToken(Usuarios usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, usuario.Username),
                new Claim("Id",Convert.ToString(usuario.Id))
                
            };

            var credenciales = new SigningCredentials(_ssKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credenciales,
                NotBefore = DateTime.Now,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public int ObtencionIdUsuario (ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return 0;
                 }
                var id = Convert.ToInt32(identity.Claims.FirstOrDefault(x => x.Type == "Id").Value);
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public TokenService(IConfiguration config)
        {
            _ssKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
        }


    }
}
