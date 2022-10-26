using Dapper;
using System.Data.Common;
using System.Data.SqlClient;

namespace RedSocial.Data
{
    public interface IUsuarioData
    {
        Task<bool> LoginUsuario(string username, string contraseña);
    }

    public class UsuarioData: IUsuarioData
    {
        private readonly string DbConnectionString;


        public UsuarioData(IConfiguration configuration)
        {
            DbConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> LoginUsuario(string username, string contraseña)
        {
            using var con = new SqlConnection(DbConnectionString);
            var existe = await con.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 
                  FROM Usuarios 
                  WHERE Username = @username AND Contraseña = @contraseña;", 
                new { username, contraseña });
            return existe == 1;
        }


    }
}
