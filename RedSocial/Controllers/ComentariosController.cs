using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ComentariosController : ControllerBase
    {
        private readonly IComentariosData comentariosData;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public ComentariosController(IComentariosData comentariosData, IMapper mapper, ITokenService tokenService)
        {
            this.comentariosData = comentariosData;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        [HttpGet("VerComentarios/{idPost}")]
        public async Task<IActionResult> GetComentarios(int idPost)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var comments = await comentariosData.VerCommentario(idPost, idUsuario);
            if(comments==null)
                return NotFound("No se encontraron comentarios hechos por usted en esta publicacion");

            return Ok(mapper.Map<ComentarioVerDTO>(comments));
        }

        [HttpPost("Comentar")]
        public async Task<IActionResult> PostComments([FromBody] ComentarioCreacionDTO comentarios)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var create = await comentariosData.CrearComentario(idUsuario, mapper.Map<Comentarios>(comentarios));
            if (create == false)
                return BadRequest("No se pudo crear el comentario");
            return Ok("El comentario fue creado exitosamente");
        }

        [HttpPut("EditarComentario/{idComentarios}")]
        public async Task<IActionResult> PutComments(int idComentarios,int idPost,[FromBody] ComentarioEditarDTO comentarios)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var existe= await comentariosData.ExisteComentario(idUsuario,idComentarios);
            if (!existe)
                return Conflict("El comentario no existe");

            var editar = await comentariosData.EditarComentario(idPost, idUsuario, idComentarios, comentarios.Comentario);
            if (!editar)
                return BadRequest("Hubo problemas al tratar de editar el post");
            return Ok("Se edito el usuario correctamente");
        }

        [HttpDelete("EliminarComentario/{idPost}/{idComentario}")]
        public async Task<IActionResult> DeleteComments(int idPost, int idComentario)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var existe = await comentariosData.ExisteComentario(idUsuario, idComentario);
            if (!existe)
                return NotFound("El comentario no existe");

            var delete = await comentariosData.EliminarComentario(idPost, idUsuario, idComentario);
            if (delete == false)
                return BadRequest("Hubo un error eliminando tu comentario");
            return Ok("Comentario borrado exitosamente");
        }
    }
}
