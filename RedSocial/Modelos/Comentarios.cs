﻿namespace RedSocial.Modelos
{
    public class Comentarios
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
    }
}
