using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Data;

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

        [HttpGet]
        public async Task<IActionResult> Get(string username, string contraseña)
        {
            var cuentaExiste = await usuarioData.LoginUsuario(username, contraseña);
            if (cuentaExiste)
            {
                return NotFound("Usuario no Encontrado");
            }
            return Ok("Bienvenido");
        }

    }
}
