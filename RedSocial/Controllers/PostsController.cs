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
    public class PostsController : ControllerBase
    {
        private readonly IPostsData postsData;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;

        public PostsController(IPostsData postsData, IMapper mapper, ITokenService tokenService)
        {
            this.postsData = postsData;
            this.mapper = mapper;
            this.tokenService = tokenService;
        }

        [HttpGet("Ver/{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {

            var rToken=tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (rToken == 0)
                return BadRequest("El token no es valido");

            var posts = await postsData.VerPost(id);

            if (posts == null)
                return NotFound("No se encontro nada");

            return Ok(posts);
        }

        [HttpGet("VerTodos/{idPost}")]
        public async Task<IActionResult> GetPost(int idPost)
        {
            var idUsuario = tokenService.ObtencionIdUsuario(HttpContext.User.Identity as ClaimsIdentity);
            if (idUsuario == 0)
                return BadRequest("El token no es valido");

            var exist = await postsData.ExistePost(idPost);
            if (!exist)
                return NotFound("No se encontro este post");

            var posts = await postsData.VerPostPorId(idUsuario, idPost);

            if (posts == null)
                return BadRequest("Hubo un inconveniente con este post");

            return Ok(posts);
        }
        [HttpPost("Crear/{id}")]
        public async Task<IActionResult> CreatePost(int id, [FromBody] PostsCrearDTO posts)
        {

            var create = await postsData.CrearPost(id, mapper.Map<Posts>(posts));

            if (!create)
                return BadRequest("No se pudo crear el usuario");


            return Ok("Post creado exitosamente");
        }

        [HttpPut("Editar/{idUsuario}")]
        public async Task<IActionResult> EditPost(int idUsuario,int idPost, [FromBody] PostsCrearDTO post)
        {
            var exist = await postsData.ExistePost(idPost);
            if (!exist)
                return NotFound("No se encontro este post");

            var edit = await postsData.EditarPost(idUsuario, idPost, mapper.Map<Posts>(post));
            if (!edit)
                return NotFound("No se pudo editar");

            return Ok("Se edito el post correctamente");
        }

        [HttpDelete("Eliminar/{idUsuario}/{idPost}")]
        public async Task<IActionResult> DeletePost(int idUsuario, int idPost)
        {
            var exist = await postsData.ExistePost(idPost);
            if (!exist)
                return NotFound("No se encontro este post");

            var delete = await postsData.EliminarPost(idUsuario, idPost);
            if (!delete)
                return BadRequest("No se pudo eliminar el post");

            return Ok("Se elimino el post correctamente");
        }

    }
}
