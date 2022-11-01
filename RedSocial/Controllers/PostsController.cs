using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RedSocial.Data;
using RedSocial.Modelos;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsData postsData;

        public PostsController(IPostsData postsData)
        {
            this.postsData = postsData;
        }

        [HttpGet("Ver/{id}")]
       public async Task<IActionResult> GetPosts(int id)
        {       
            var posts = await postsData.VerPost(id);

            if(posts == null)
                return NotFound("No se encontro nada");

            return Ok(posts);
        }

        [HttpGet("Ver/{idUsuario}/{idPost}")]
        public async Task<IActionResult> GetPost(int idUsuario, int idPost)
        {

            var exist = await postsData.ExistePost(idPost);
            if(!exist)
                return NotFound("No se encontro este post");

            var posts = await postsData.VerPostPorId(idUsuario, idPost);

            if (posts == null)
                return BadRequest("Hubo un inconveniente con este post");

            return Ok(posts);
        }
        [HttpPost("Crear/{id}")]
        public async Task<IActionResult> CreatePost(int id, [FromBody]Posts posts)
        {

            var create= await postsData.CrearPost(id, posts);

            if (!create)
                return BadRequest("No se pudo crear el usuario");


            return Ok("Post creado exitosamente");
        }

        [HttpPut("Editar/{idUsuario}/{idPost}")]
        public async Task<IActionResult> EditPost (int idUsuario, int idPost, [FromBody] Posts post)
        {
            var edit = await postsData.EditarPost(idUsuario, idPost, post);
            if (!edit)
                return BadRequest("No se pudo editar el post");

            return Ok("Se edito el post correctamente");
        }

        [HttpDelete("Eliminar/{idUsuario}/{idPost}")]
        public async Task<IActionResult> DeletePost(int idUsuario, int idPost)
        {
            var delete = await postsData.EliminarPost(idUsuario, idPost);
            if (!delete)
                return BadRequest("No se pudo eliminar el post");

            return Ok("Se elimino el post correctamente");
        }

    }
}
