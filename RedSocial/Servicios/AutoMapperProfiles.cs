using AutoMapper;
using RedSocial.Modelos;
using RedSocial.Modelos.DTOs;

namespace RedSocial.Servicios
{
    public class AutoMapperProfiles: Profile
    {

        public AutoMapperProfiles()
        {
            //----------Usuario-------------
            //Create
            CreateMap<UsuarioCreacionDTO,Usuarios>();

            //----------Post-----------------
            //Create
            CreateMap<PostsCrearDTO, Posts>();
            //Ver
            CreateMap<Posts, PostsVerDTO>();

            //---------Comentario-----------
            //Create
            CreateMap<ComentarioCreacionDTO,Comentarios>();
            //Editar
            CreateMap<ComentarioEditarDTO, Comentarios>();
            //Ver
            CreateMap<Comentarios,ComentarioVerDTO>();

            //----------Reaccion----------
            //Create
            CreateMap<ReaccionesCreacionDTO, Reacciones>();
            //Editar
            CreateMap<ReaccionesEditarDTO, Reacciones>();
            //Ver
            CreateMap<Reacciones,ReaccionesVerDTO>();
        }

    }
}
