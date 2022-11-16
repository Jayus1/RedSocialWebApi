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
    }

    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _ssKey;
        public string CreateToken(Usuarios usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.Username)
               // new Claim("id", usuario.id),
            };

            var credenciales = new SigningCredentials(_ssKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credenciales,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public TokenService(IConfiguration config)
        {
            _ssKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
        }


    }
}
