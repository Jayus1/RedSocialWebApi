using Dapper;
using Microsoft.Data.SqlClient;
using RedSocial.Modelos;

namespace RedSocial.Data
{
    public class PostsData
    {
        private readonly string connectionString;

        public PostsData(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Posts> VerPost(int id)
        {
            using var conn = new SqlConnection(connectionString);
            var posts = conn.Query<Posts>("SELECT Titulo, Contenido From Posts WHERE IdUsuario = @Id", new { id });
            return posts;
        }

        public IEnumerable<Posts> VerPostPorId(int idUsuario, int idPost)
        {
            using var conn = new SqlConnection(connectionString);
            var posts = conn.QueryFirstOrDefault<Posts>("SELECT Titulo,Contenido From Posts WHERE IdUsuario = @idUsuario and Id= @idPost", new { idUsuario, idPost });
            return posts;
        }

        public Task<bool> CrearPost
        {

        }

        public async Task<bool> EditarPost(int idPost, int idUsuario, Posts post)
        {
            using var conn = new SqlConnection(connectionString):
                var editar = await conn.ExecuteAsync("UPDATE Posts SET Titulo=@titulo, Contenido=@contenido WHERE Id=@IdPost AND Id=@IdUsuario", new { post, idPost, idUsuario });

            if(editar !=1)
                return false;

            return true;
        }
    }
}
