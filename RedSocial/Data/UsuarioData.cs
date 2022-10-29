using Dapper;
using RedSocial.Modelos;
using System.Data.Common;
using System.Data.SqlClient;

namespace RedSocial.Data
{
    public interface IUsuarioData
    {
        Task<bool> CreacionDeUsuario(Usuarios usuario);
        Task<bool> ExistenciaUsuario(string username);
        Task<bool> LoginUsuario(string username, string contraseña);
    }

    public class UsuarioData: IUsuarioData
    {
        private readonly string connectionString;

        public UsuarioData(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> LoginUsuario(string username, string contraseña)
        {
            using var con = new SqlConnection(connectionString);
            var existe = await con.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 
                  FROM Usuarios 
                  WHERE Username = @username AND Contraseña = @contraseña;", 
                new { username, contraseña });
            return existe == 1;
        }

        public async Task<bool> CreacionDeUsuario(Usuarios usuario)
        {
            using var con= new SqlConnection(connectionString);
            var crear = await con.ExecuteAsync("INSERT INTO Usuarios " +
                                                "VALUES (@Username, @contraseña);",
                                                usuario);
            if(crear != 1)
                return false;

            return true;
        }

        public async Task<bool> ExistenciaUsuario(string username)
        {
            using var con = new SqlConnection(connectionString);
            var existe = await con.ExecuteAsync(
                @"SELECT 1 
                  FROM Usuarios 
                  WHERE Username = @username",
                new { username });
                
            
            if(existe != 1)
                return false;

            return true;

        }


    }
}
