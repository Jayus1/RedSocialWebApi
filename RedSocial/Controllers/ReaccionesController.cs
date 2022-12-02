using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using RedSocial.Data;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;
using RedSocial.Servicios;
using System.Security.Claims;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReaccionesController : ControllerBase
    {
        private readonly IReaccionesData reaccionesData;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public ReaccionesController(IReaccionesData reaccionesData, IMapper mapper, ITokenService tokenService)
        {
            this.reaccionesData = reaccionesData;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        [HttpGet("{idPost}")]
        public async Task<IActionResult> GetReacciones(int idPost)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var exist = await reaccionesData.ExisteReaccion(idUsuario, idPost);
            if (!exist)
                return Conflict("Esta reaccion no existe");

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
        public async Task<IActionResult> CreateReacciones([FromBody] ReaccionesCreacionDTO react)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var exist = await reaccionesData.ExisteReaccion(idUsuario, react.IdPost);
            if (exist)
                return Conflict("Esta reaccion ya fue hecha");

            var create = await reaccionesData.CrearReaccion(idUsuario, mapper.Map<Reacciones>(react));
            if (!create)
                return BadRequest("Hubo un error al intentar crear la reaccion");

            return Ok("Se ha creado la reaccion correctamente");
        }

        [HttpPut("EditarReaccion/{idPost}")]
        public async Task<IActionResult> EditReacciones([FromBody] ReaccionesEditarDTO react, int idPost)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var exist = await reaccionesData.ExisteReaccion(idUsuario, idPost);
            if (!exist)
                return Conflict("Esta reaccion no existe");

            var create = await reaccionesData.EditarReaccion(mapper.Map<Reacciones>(react),idUsuario, idPost);
            if (!create)
                return BadRequest("Hubo un error al intentar editar la reaccion");

            return Ok("Se ha editado la reaccion correctamente");
        }

        [HttpDelete("EliminarReaccion/{idPost}")]
        public async Task<IActionResult> DeleteReaccion(int idPost)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var exist = await reaccionesData.ExisteReaccion(idUsuario, idPost);
            if (!exist)
                return Conflict("Esta reaccion no existe");

            var delete= await reaccionesData.DeleteReaccion(idUsuario, idPost);
            if (!delete)
                return BadRequest("Error al eliminar la reaccion");
            return Ok("Se elimino la reaccion correctamente");
        }
    }
}
