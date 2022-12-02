using Dapper;
using Microsoft.Data.SqlClient;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;

namespace RedSocial.Data
{
    public interface IReaccionesData
    {
        Task<bool> CrearReaccion(int idUsuario,Reacciones reaccion);
        Task<bool> DeleteReaccion(int idUser, int idPost);
        Task<bool> EditarReaccion(Reacciones reaccion, int idUsuario, int idPost);
        Task<bool> ExisteReaccion(Reacciones reaccion);
        Task<bool> ExisteReaccion(int idUser, int idPost);
        Task<IEnumerable<ReaccionesVerDTO>> VerReaccion(int idPost);
        Task<IEnumerable<TiposDeReacciones>> VerTiposReaccion();
    }

    public class ReaccionesData : IReaccionesData
    {
        private readonly string connectionstring;

        public ReaccionesData(IConfiguration configuration) => connectionstring = configuration.GetConnectionString("DefaultConnection");

        public async Task<bool> CrearReaccion(int idUsuario, Reacciones reaccion)
        {
            using var cnn = new SqlConnection(connectionstring);
            var crear = await cnn.ExecuteAsync(@"Insert INTO Reacciones 
                                                 Values (@IdPost,@IdUsuario,@IdTipoReaccion)", 
                                                 new {reaccion.IdPost, idUsuario, reaccion.IdTipoReaccion});

            if (crear != 1)
                return false;

            return true;
        }


        public async Task<bool> EditarReaccion(Reacciones reaccion, int idUsuario, int idPost)
        {
            using var cnn = new SqlConnection(connectionstring);
            var edit = await cnn.ExecuteAsync(@"UPDATE Reacciones 
                                               SET IdTipoReaccion= @idTipoReaccion 
                                               WHERE IdUsuario= @idUsuario
                                               AND IdPost= @idPost", 
                                               new {reaccion.IdTipoReaccion, idUsuario, idPost});
           

            if (edit != 1)
                return false;

            return true;
        }

        public async Task<bool> DeleteReaccion(int idUser, int idPost)
        {
            using var cnn = new SqlConnection(connectionstring);
            var delete = await cnn.ExecuteAsync(@"DELETE FROM Reacciones 
                                                  WHERE IdPost= @idPost 
                                                  AND IdUsuario= @idUser ", 
                                                  new { idUser, idPost });

            if (delete != 1)
                return false;

            return true;
        }

        public async Task<IEnumerable<ReaccionesVerDTO>> VerReaccion(int idPost)
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryAsync<ReaccionesVerDTO>(@"SELECT * FROM Reacciones 
                                                                  WHERE IdPost= @idPost", 
                                                                  new { idPost });

            return reacts;
        }

        public async Task<IEnumerable<TiposDeReacciones>> VerTiposReaccion()
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryAsync<TiposDeReacciones>(@"SELECT * FROM TiposDeReacciones");

            return reacts;
        }

        public async Task<bool> ExisteReaccion(Reacciones reaccion)
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryFirstOrDefaultAsync<Reacciones>(@"SELECT *  FROM Reacciones 
                                                                          WHERE IdUsuario= @idUsuario
                                                                          AND IdPost= @idPost",
                                                                          reaccion);
            if (reacts == null)
                return false;
            return true;
        }
        public async Task<bool> ExisteReaccion(int idUser, int idPost)
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryFirstOrDefaultAsync<Reacciones>(@"SELECT * FROM Reacciones 
                                                                          WHERE IdUsuario= @idUser 
                                                                          AND IdPost= @idPost",
                                                                          new { idUser,idPost});
            if (reacts is null)
                return false;
            return true;
        }
    }
}
