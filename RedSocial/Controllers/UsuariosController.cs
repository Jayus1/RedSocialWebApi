using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Data;
using RedSocial.Modelos;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioData usuarioData;

        public UsuariosController(IUsuarioData usuarioData)
        {
            this.usuarioData = usuarioData;
        }

        [HttpPost(Name ="LoginDeUsuarios")]
        public async Task<IActionResult> Login([FromBody] Usuarios usuario)
        {
            var cuentaExiste = await usuarioData.LoginUsuario(usuario.Username, usuario.Contraseña);
            if (!cuentaExiste)
            {
                return NotFound("Usuario no Encontrado");
            }
            return Ok("Bienvenido");
        }

        [HttpPost(Name ="CrearUsuarios")]
        public async Task<IActionResult> CreateUser([FromBody] Usuarios usuario)
        {
            var cuentaExiste = await usuarioData.LoginUsuario(usuario.Username, usuario.Contraseña);
            if (!cuentaExiste)
            {
                return BadRequest("El usuario ya existe");
            }

            await usuarioData.CreacioDeUsuario(usuario);
            return Ok("Bienvenido");

        }

    }
}
