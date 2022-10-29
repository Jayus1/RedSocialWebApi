using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet("{id}")]
       public async Task<IActionResult> GetPosts(int id)
        {
            var posts = await VerPosts(id);

            return Ok();
        }


    }
}
