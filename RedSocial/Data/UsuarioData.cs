using Dapper;
using Microsoft.Extensions.Hosting;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;
using System.Data.Common;
using System.Data.SqlClient;

namespace RedSocial.Data
{
    public interface IUsuarioData
    {
        Task<bool> CreacionDeUsuario(UsuarioCreacionDTO usuario);
        Task<bool> EditarUsuario(int id, UsuarioCreacionDTO usuarioCreacionDTO);
        Task<bool> EliminarUsuario(int id);
        Task<bool> ExistenciaUsuario(string username);
        Task<bool> ExistenciaUsuario(int id);
        Task<Usuarios> LoginUsuario(UsuarioCreacionDTO usuario);
    }

    public class UsuarioData: IUsuarioData
    {
        private readonly string connectionString;

        public UsuarioData(IConfiguration configuration) => connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task<Usuarios> LoginUsuario(UsuarioCreacionDTO usuario)
        {
            using var con = new SqlConnection(connectionString);
            var login = await con.QueryFirstOrDefaultAsync<Usuarios>(
                @"SELECT *
                  FROM Usuarios 
                  WHERE Username = @Username AND Contraseña = @Contraseña;", 
                usuario);
            return login;
        }

        public async Task<bool> CreacionDeUsuario(UsuarioCreacionDTO usuario)
        {
            using var con= new SqlConnection(connectionString);

            var existe =await  ExistenciaUsuario(usuario.Username);
            if(existe)
                return false;
            
            var crear = await con.ExecuteAsync(@"INSERT INTO Usuarios
                                                VALUES (@Username, @contraseña);",
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
        public async Task<bool> ExistenciaUsuario(int id)
        {
            using var con = new SqlConnection(connectionString);
            var existe = await con.QueryFirstOrDefaultAsync<Usuarios>(
                @"SELECT Username, Contraseña 
                  FROM Usuarios 
                  WHERE Id = @id",
                new { id });


            if (existe is null)
                return false;

            return true;

        }

        public async Task<bool> EditarUsuario(int id,UsuarioCreacionDTO usuarioCreacionDTO)
        {
            using var con= new SqlConnection(connectionString);
            var editar = await con.ExecuteAsync(@"UPDATE Usuarios 
                                                   SET Username= @Username, Contraseña= @Contraseña 
                                                   WHERE Id=@id",
                                                   new { usuarioCreacionDTO.Username, usuarioCreacionDTO.Contraseña, id });

            if (editar != 1)
                return false;

            return true;
        }

        public async Task<bool> EliminarUsuario(int id)
        {
            using var con = new SqlConnection(connectionString);
            var delete = await con.ExecuteAsync(@"DELETE From Usuarios 
                                                  WHERE Id = @id", new { id });
                                    
            if (delete != 1)
                return false;
            return true;
        }
    }
}
