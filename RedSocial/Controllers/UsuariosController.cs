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
        private readonly UsuarioData usuarioData;

        public UsuariosController(UsuarioData usuarioData)
        {
            this.usuarioData = usuarioData;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] Usuarios usuario)
        {
            var cuentaExiste = await usuarioData.LoginUsuario(usuario.Username, usuario.Contraseña);
            if (cuentaExiste)
            {
                return NotFound("Usuario no Encontrado");
            }
            return Ok("Bienvenido");
        }

    }
}
