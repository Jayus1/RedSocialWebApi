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

        [HttpPost("Login",Name = "LoginDeUsuarios")]
        //[Route("Login")]
        public async Task<IActionResult> Login([FromBody] Usuarios usuario)
        {
            var cuentaExiste = await usuarioData.LoginUsuario(usuario.Username, usuario.Contraseña);
            if (!cuentaExiste)
                return NotFound("Usuario no Encontrado");

            return Ok("Bienvenido");
        }

        [HttpPost("Registro", Name = "CrearUsuarios")]
        //[Route("Registro")]
        public async Task<IActionResult> CreateUser([FromBody] Usuarios usuario)
        {
            var cuentaExiste = await usuarioData.ExistenciaUsuario(usuario.Username);
            if (!cuentaExiste)          
                return Conflict("El usuario ya existe");
            

            var crear=await usuarioData.CreacionDeUsuario(usuario);

            if (!crear)
                return BadRequest("Hubo un conflicto con la creacion de su usuario");

            return Ok("Usuario creado correctamente");

        }

    }
}
