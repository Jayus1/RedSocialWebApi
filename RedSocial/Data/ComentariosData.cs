using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RedSocial.Modelos;

namespace RedSocial.Data
{
    public interface IComentariosData
    {
        Task<bool> CrearComentario(int idUsuario, int idPost, [FromBody] Comentarios comentarios);
        Task<bool> EditarComentario(int idPost, int idUsuario, int idComentario, [FromBody] Comentarios comentarios);
        Task<bool> EliminarComentario(int idPost, int idUsuario, int idComentario);
        Task<bool> ExisteComentario(int idUsuario, int idComentario);
        Task<Comentarios> VerCommentario(int idPost, int idUsuario);
    }

    public class ComentariosData : IComentariosData
    {
        private readonly string connectionstring;

        public ComentariosData(IConfiguration configuration) => connectionstring = configuration.GetConnectionString("DefaultConnection");

        public async Task<Comentarios> VerCommentario(int idPost, int idUsuario)
        {
            using var conn = new SqlConnection(connectionstring);
            var comentario = await conn.QueryFirstOrDefaultAsync<Comentarios>(@"SELECT Comentario
                                                                                FROM Comentarios
                                                                                WHERE IdPost= @idPost
                                                                                AND IdUsuario= @idUsuario",
                                                                                new { idPost, idUsuario });
            return comentario;
        }

        public async Task<bool> CrearComentario(int idUsuario, int idPost, Comentarios comentarios)
        {
            using var conn = new SqlConnection(connectionstring);
            var creacion = await conn.ExecuteAsync(@"INSERT INTO Comentarios 
                                               VALUES (@idPost, @idUsuario, @comentario)",
                                               new { idPost, idUsuario, comentarios.Comentario });
            if (creacion == null)
                return false;
            return true;
        }

        public async Task<bool> EditarComentario(int idPost, int idUsuario, int idComentario, Comentarios comentarios)
        {
            using var conn = new SqlConnection(connectionstring);
            var editar = await conn.ExecuteAsync(@"UPDATE INTO Comentarios 
                                                   VALUEES Comentario= @comentarios
                                                   WHERE IdPost = @idPost AND IdUsuario= @idUsuario;",
                                                   new { comentarios.Comentario, idPost, idUsuario });
            if (editar != 1)
                return false;

            return true;
        }

        public async Task<bool> EliminarComentario(int idPost, int idUsuario, int idComentario)
        {
            using var conn = new SqlConnection(connectionstring);
            var eliminar = await conn.ExecuteAsync(@"DELETE FROM Comentarios
                                                     WHERE IdPost= @idPost
                                                     AND IdUsuario= @idUsuario
                                                     AND Id= @idComentario;",
                                                     new { idPost, idUsuario, idComentario });
            if (eliminar != 1)
                return false;

            return true;
        }

        public async Task<bool> ExisteComentario(int idUsuario, int idComentario)
        {
            using var conn = new SqlConnection(connectionstring);
            var existe = await conn.ExecuteAsync(@"SELECT * FROM Comentarios 
                                                   WHERE Id = @idComentario
                                                   AND IdUsuario = @@idUsuario;",
                                                   new { idComentario, idUsuario });
            if (existe != 1)
                return false;
            return true;
        }

    }
}
