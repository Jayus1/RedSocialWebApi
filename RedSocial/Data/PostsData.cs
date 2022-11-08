using Dapper;
using Microsoft.Data.SqlClient;
using RedSocial.Modelos;

namespace RedSocial.Data
{
    public interface IPostsData
    {
        Task<bool> CrearPost(int id, Posts post);
        Task<bool> EditarPost(int idPost, Posts post);
        Task<bool> EliminarPost(int idUsuario, int idPost);
        Task<bool> ExistePost(int idPost);
        Task<IEnumerable<Posts>> VerPost(int id);
        Task<Posts> VerPostPorId(int idUsuario, int idPost);
    }

    public class PostsData : IPostsData
    {
        private readonly string connectionString;

        public PostsData(IConfiguration configuration) => connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task<IEnumerable<Posts>> VerPost(int id)
        {
            using var conn = new SqlConnection(connectionString);
            var posts = await conn.QueryAsync<Posts>(@"SELECT Titulo, Contenido 
                                                       From Posts 
                                                       WHERE IdUsuario = @Id", 
                                                       new { id });
            return posts;
        }

        public async Task<Posts> VerPostPorId(int idUsuario, int idPost)
        {
            using var conn = new SqlConnection(connectionString);
            var posts = await conn.QueryFirstOrDefaultAsync<Posts>(@"SELECT Titulo,Contenido 
                                                                     From Posts 
                                                                     WHERE IdUsuario = @idUsuario and Id= @idPost", 
                                                                     new { idUsuario, idPost });
            return posts;
        }

        public async Task<bool> CrearPost(int idUsuario, Posts post)
        {
            using var con = new SqlConnection(connectionString);
            var create = await con.ExecuteAsync(@"INSERT INTO Posts 
                                                  VALUES (@Titulo, @Contenido, @idUsuario);", 
                                                  new { post.Titulo, post.Contenido, idUsuario });

            return true;
        }

        public async Task<bool> EditarPost(int idUsuario, Posts post)
        {
            using var conn = new SqlConnection(connectionString);
            var editar = await conn.ExecuteAsync(@"UPDATE Posts 
                                                   SET Titulo=@titulo, Contenido=@contenido 
                                                   WHERE Id=@idPost AND IdUsuario=@idUsuario", 
                                                   new { post.Titulo,post.Contenido, idPost=post.Id, idUsuario});

            if (editar != 1)
                return false;

            return true;
        }

        public async Task<bool> EliminarPost(int idUsuario, int idPost)
        {
            using var con = new SqlConnection(connectionString);
            var delete = await con.ExecuteAsync(@"DELETE From Posts 
                                                  WHERE Id= @idPost AND IdUsuario= @idUsuario", 
                                                  new { idPost, idUsuario });
            if (delete != 1)
                return false;
            return true;
        }

        public async Task<bool> ExistePost(int idPost)
        {
            using var con = new SqlConnection(connectionString);
            var existe = await con.QueryFirstAsync<Posts>(@"SELECT * FROM Posts
                                                  WHERE Id = @idPost",
                                                  new { idPost });
            if (existe is null)
                return false;

            return true;
        }
    }
}
