using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RedSocial.Data;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;
using RedSocial.Servicios;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioData usuarioData;
        private readonly IConfiguration configuration;
        private readonly ITokenService token;

        public UsuariosController(IUsuarioData usuarioData, IConfiguration configuration, ITokenService token)
        {
            this.usuarioData = usuarioData;
            this.configuration = configuration;
            this.token = token;
        }

        [HttpPost("Login",Name = "LoginDeUsuarios")]
        public async Task<IActionResult> Login([FromBody] UsuarioCreacionDTO usuario)
        {
            var cuentaExiste = await usuarioData.LoginUsuario(usuario);
            if (cuentaExiste is null)
                return NotFound("Usuario no Encontrado");

            var tokenf=token.CreateToken(cuentaExiste); 

            return Ok(new
            {
                token = tokenf,
                Mensaje="Se ha logeado correctamente"
            });
        }

        [HttpPost("Creacion", Name = "CrearUsuarios")]
        public async Task<IActionResult> CreateUser([FromBody] UsuarioCreacionDTO usuario)
        {
            
            var cuentaExiste = await usuarioData.ExistenciaUsuario(usuario.Username);
            if (cuentaExiste)          
                return Conflict("El usuario ya existe");
            

            var crear=await usuarioData.CreacionDeUsuario(usuario);

            if (!crear)
                return BadRequest("Hubo un conflicto con la creacion de su usuario");

            return Ok("Usuario creado correctamente");

        }

    }
}
