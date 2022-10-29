namespace RedSocial.Modelos
{
    public class Posts
    {
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public string Contenido { get; set; }

        [NonSerialized]
        public int IdUsuario { get; set; }
    }
}
