using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedSocial.Modelos;

namespace RedSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        [HttpGet]
        public async Task<Comentarios> GetComentarios()
        {

        }
    }
}
