using Dapper;
using Microsoft.Data.SqlClient;
using RedSocial.Modelos;

namespace RedSocial.Data
{
    public interface IReaccionesData
    {
        Task<bool> CrearReaccion(Posts posts);
        Task<bool> DeleteReaccion(int idUser, int idPost, int idReaccion);
        Task<bool> EditarReaccion(Posts posts);
        Task<bool> ExisteReaccion(Posts posts);
        Task<bool> ExisteReaccion(int idUser, int idPost, int idReaccion);
        Task<IEnumerable<Reacciones>> VerReaccion(int idPost);
        Task<IEnumerable<Reacciones>> VerTiposReaccion();
    }

    public class ReaccionesData : IReaccionesData
    {
        private readonly string connectionstring;

        public ReaccionesData(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> CrearReaccion(Posts post)
        {
            using var cnn = new SqlConnection(connectionstring);
            var crear = await cnn.ExecuteAsync("Insert INTO Reacciones Values (@idPost,@idUser,@idReaccion)", new {post});

            if (crear != 1)
                return false;

            return true;
        }


        public async Task<bool> EditarReaccion(Posts posts)
        {
            using var cnn = new SqlConnection(connectionstring);
            var edit = await cnn.ExecuteAsync("UPDATE Reacciones SET IdTipoReaccion= @idReaccion WHERE IdUsuario= @idUser AND IdPost= @idPosr)", new {posts});

            if (edit != 1)
                return false;

            return true;
        }

        public async Task<bool> DeleteReaccion(int idUser, int idPost, int idReaccion)
        {
            using var cnn = new SqlConnection(connectionstring);
            var delete = await cnn.ExecuteAsync("DELETE Reacciones WHERE IdTipoReaccion= @idReaccion AND IdUsuario= @idUser AND IdPost= @idPosr)", new { idReaccion, idUser, idPost });

            if (delete != 1)
                return false;

            return true;
        }

        public async Task<IEnumerable<Reacciones>> VerReaccion(int idPost)
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryAsync<Reacciones>("SELECT * FROM Reacciones WHERE IdPost= @idPosr)", idPost);

            return reacts;
        }

        public async Task<IEnumerable<Reacciones>> VerTiposReaccion()
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryAsync<Reacciones>("SELECT * FROM TiposDeReacciones)");

            return reacts;
        }

        public async Task<bool> ExisteReaccion(Posts posts)
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryFirstOrDefaultAsync<Reacciones>(@"SELECT * FROM Reacciones 
                                                                          WHERE IdUsuario= @idUser 
                                                                          AND IdReaccion= @idReaccion 
                                                                          AND IdPost= @idPosr)",
                                                                          new { posts});
            if (reacts == null)
                return false;
            return true;
        }
        public async Task<bool> ExisteReaccion(int idUser, int idPost, int idReaccion)
        {
            using var cnn = new SqlConnection(connectionstring);
            var reacts = await cnn.QueryFirstOrDefaultAsync<Reacciones>(@"SELECT * FROM Reacciones 
                                                                          WHERE IdUsuario= @idUser 
                                                                          AND IdReaccion= @idReaccion 
                                                                          AND IdPost= @idPosr)",
                                                                          new { idUser,idReaccion, idPost });
            if (reacts == null)
                return false;
            return true;
        }
    }
}
