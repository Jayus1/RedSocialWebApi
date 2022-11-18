using Dapper;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;
using System.Data.Common;
using System.Data.SqlClient;

namespace RedSocial.Data
{
    public interface IUsuarioData
    {
        Task<bool> CreacionDeUsuario(UsuarioCreacionDTO usuario);
        Task<bool> ExistenciaUsuario(string username);
        Task<bool> LoginUsuario(UsuarioCreacionDTO usuario);
    }

    public class UsuarioData: IUsuarioData
    {
        private readonly string connectionString;

        public UsuarioData(IConfiguration configuration) => connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task<bool> LoginUsuario(UsuarioCreacionDTO usuario)
        {
            using var con = new SqlConnection(connectionString);
            var login = await con.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 
                  FROM Usuarios 
                  WHERE Username = @Username AND Contraseña = @Contraseña;", 
                new { usuario });
            return login == 1;
        }

        public async Task<bool> CreacionDeUsuario(UsuarioCreacionDTO usuario)
        {
            using var con= new SqlConnection(connectionString);

            var existe =await  ExistenciaUsuario(usuario.Username);
            if(existe)
                return false;
            
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
            var existe = await con.QueryFirstOrDefaultAsync<Usuarios>(
                @"SELECT Username, Contraseña 
                  FROM Usuarios 
                  WHERE Username = @username",
                new {username});
                
            
            if(existe is null)
                return false;

            return true;

        }
    }
}
