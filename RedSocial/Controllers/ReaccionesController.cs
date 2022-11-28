using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using RedSocial.Data;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ReaccionesController : ControllerBase
    {
        private readonly IReaccionesData reaccionesData;
        private readonly IMapper mapper;

        public ReaccionesController(IReaccionesData reaccionesData, IMapper mapper)
        {
            this.reaccionesData = reaccionesData;
            this.mapper = mapper;
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

        [HttpPost("CrearReaccciones/{idUser}")]
        public async Task<IActionResult> CreateReacciones([FromBody] ReaccionesCreacionDTO react, int idUser)
        {
            var exist = await reaccionesData.ExisteReaccion(idUser, react.IdPost);
            if (exist)
                return Conflict("Esta reaccion ya fue hecha");

            var create = await reaccionesData.CrearReaccion(idUser,mapper.Map<Reacciones>(react));
            if (!create)
                return BadRequest("Hubo un error al intentar crear la reaccion");

            return Ok("Se ha creado la reaccion correctamente");
        }

        [HttpPut("EditarReacciones/{idUsuario}/{idPost}")]
        public async Task<IActionResult> EditReacciones([FromBody] ReaccionesEditarDTO react, int idUsuario, int idPost)
        {
            var exist = await reaccionesData.ExisteReaccion(idUsuario, idPost);
            if (!exist)
                return Conflict("Esta reaccion no existe");

            var create = await reaccionesData.EditarReaccion(mapper.Map<Reacciones>(react),idUsuario, idPost);
            if (!create)
                return BadRequest("Hubo un error al intentar editar la reaccion");

            return Ok("Se ha editado la reaccion correctamente");
        }

        [HttpDelete("{idUser}/{idPost}/{idReaccion}")]
        public async Task<IActionResult> DeleteReaccion(int idUser, int idPost, int idReaccion)
        {
            var exist = await reaccionesData.ExisteReaccion(idUser, idPost);
            if (!exist)
                return Conflict("Esta reaccion no existe");

            var delete= await reaccionesData.DeleteReaccion(idUser, idPost, idReaccion);
            if (!delete)
                return BadRequest("Error al eliminar la reaccion");
            return Ok("Se elimino la reaccion correctamente");
        }

    }
}
