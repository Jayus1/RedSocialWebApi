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

        //public static dynamic validarToken(ClaimsIdentity identity)
        //{
        //    try
        //    {
        //        if (identity.Claims.Count() == 0)
        //            return new
        //            {
        //                success= false,
        //                message= "Verificar si estas enviando un token valido",
        //                result=""
        //            };

        //        var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
        //        Usuarios usuario = usuario
        //    }
        //    catch (Exception ex) 
        //    {
        //        return new
        //        {
        //            success = false,
        //            message = "Catch: " + ex.Message,
        //            result = ""
        //        };
        //    }
        //}


        public TokenService(IConfiguration config)
        {
            _ssKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
        }


    }
}
