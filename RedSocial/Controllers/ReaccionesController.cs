using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using RedSocial.Data;
using RedSocial.Modelos;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaccionesController : ControllerBase
    {
        private readonly IReaccionesData reaccionesData;

        public ReaccionesController(IReaccionesData reaccionesData)
        {
            this.reaccionesData = reaccionesData;
        }

        [HttpGet("{idPost}")]
        public async Task<IActionResult> GetReacciones(int idPost)
        {
            
            var ver = await reaccionesData.VerReaccion(idPost);
            if(ver == null)
                return NotFound("No se encontraron reacciones");
            return Ok(ver);
        }

        [HttpGet("TiposDeReacciones")]
        public async Task<IActionResult> GetTiposReacciones()
        {

            var ver = await reaccionesData.VerTiposReaccion();
            if (ver == null)
                return NotFound("No se encontraron reacciones");
            return Ok(ver);
        }

        [HttpPost("CrearReaccciones")]
        public async Task<IActionResult> CreateReacciones([FromBody] Reacciones react)
        {
            var exist = await reaccionesData.ExisteReaccion(react);
            if (exist)
                return Conflict("Esta reaccion ya fue hecha");

            var create = await reaccionesData.CrearReaccion(react);
            if (!create)
                return BadRequest("Hubo un error al intentar crear la reaccion");

            return Ok("Se ha creado la reaccion correctamente");
        }

        [HttpPut("EditarReacciones")]
        public async Task<IActionResult> EditReacciones([FromBody] Reacciones react)
        {
            var exist = await reaccionesData.ExisteReaccion(react);
            if (!exist)
                return Conflict("Esta reaccion no existe");

            var create = await reaccionesData.EditarReaccion(react);
            if (!create)
                return BadRequest("Hubo un error al intentar editar la reaccion");

            return Ok("Se ha editado la reaccion correctamente");
        }

        [HttpDelete("{idUser}/{idPost}/{idReaccion}")]
        public async Task<IActionResult> DeleteReaccion(int idUser, int idPost, int idReaccion)
        {
            var exist = await reaccionesData.ExisteReaccion(idUser, idPost, idReaccion);
            if (exist)
                return Conflict("Esta reaccion ya fue hecha");

            var delete= await reaccionesData.DeleteReaccion(idUser, idPost, idReaccion);
            if (delete)
                return BadRequest("Error al eliminar la reaccion");
            return Ok("Se elimino la reaccion correctamente");
        }

    }
}
