namespace RedSocial.Data
{
    public class ComentariosData
    {
        private readonly string connectionstring;

        public ComentariosData(IConfiguration configuration) => connectionstring = configuration.GetConnectionString("DefaultConnection");

        public async Task<bool> GetCommentarios(int idPost, int idUsuario)
        {

        }


    }
}
