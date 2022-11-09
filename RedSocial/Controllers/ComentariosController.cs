﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Data;
using RedSocial.Modelos;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentariosData comentariosData;

        public ComentariosController(IComentariosData comentariosData) => this.comentariosData = comentariosData;

        [HttpGet("VerComentarios/{idUsuario}/{idPost}")]
        public async Task<IActionResult> GetComentarios(int idUsuario, int idPost)
        {
            
            var comments = await comentariosData.VerCommentario(idPost, idUsuario);
            if(comments==null)
                return NotFound("No se encontraron comentarios hechos por usted en esta publicacion");

            return Ok(idPost);
        }

        [HttpPost("Comentar/{idUsuario}")]
        public async Task<IActionResult> PostComments(int idUsuario, [FromBody] Comentarios comentarios)
        {

            var create = await comentariosData.CrearComentario(idUsuario, comentarios);
            if (create == false)
                return BadRequest("No se pudo crear el comentario");
            return Ok("El post fue creado exitosamente");
        }

        [HttpPut("EditarComentario/{idUsuario}/{idComentarios}")]
        public async Task<IActionResult> PutComments(int idUsuario ,int idComentarios,[FromBody] Comentarios comentarios)
        {
            var existe= await comentariosData.ExisteComentario(idUsuario,idComentarios);
            if (!existe)
                return Conflict("El comentario no existe");

            var editar = await comentariosData.EditarComentario(comentarios.IdPost, idUsuario, idComentarios, comentarios.Comentario);
            if (!editar)
                return BadRequest("Hubo problemas al tratar de editar el post");
            return Ok("Se edito el usuario correctamente");
        }

        [HttpDelete("EliminarComentario/{idUsuario}/{idPost}/{idComentario}")]
        public async Task<IActionResult> DeleteComments(int idPost,int idUsuario, int idComentario)
        {
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