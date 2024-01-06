using AutoMapper;
using DashboardDemo.Entities.DTOs;
using DashboardDemo.Entities.DTOs.Mesas;
using DashboardDemo.Entities.DTOs.Turnos;
using DashboardDemo.Entities.DTOs.Ventas;
using DashboardDemo.Entities.Identity.Users;
using DashboardDemo.Entities.Models;

namespace DashboardDemo.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // AllowNullCollections
            // AllowNullDestinationValues
            AllowNullDestinationValues = true;
            // CreateMap<Source, Destination>();
            // user
            //CreateMap<ApplicationUser, UserDto>(); ;
            //CreateMap<UserForRegistrationDto, ApplicationUser>();
            CreateMap<UserCredentials, ApplicationUser>();
            // role
            //CreateMap<ApplicationRole, RoleDto>();
            //CreateMap<RoleCreateOrUpdate, ApplicationRole>();
            // user role
            //CreateMap<ApplicationUserRole, UserRoleDto>();

            //PersonalForRegistrationDto a ApplicationUser.
            CreateMap<PersonalCreateDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DNI));
            /*
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore());
            */
            //ApplicationUser a PersonalDto
            CreateMap<ApplicationUser, PersonalReadDto>()
                .ForMember(dest => dest.DNI, opt => opt.MapFrom(src => src.UserName));
            
            CreateMap<PersonalUpdateDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.DNI)).ReverseMap();

            //Mesa
            CreateMap<MesaCreateDto, Mesa>();
            CreateMap<Mesa,MesaRetrieveDto>();
            CreateMap<MesaBaseDto, Mesa>();

            //Venta
            CreateMap<VentaCreateDto, Venta>()
            .ForMember(dest => dest.VentaId, opt => opt.Ignore()); // Ignorar la propiedad Id si es necesario
            CreateMap<Venta, VentaCreateDto>();

            //Turno
            CreateMap<TurnoCreateDto, Turno>().ReverseMap();
            CreateMap<Turno, TurnoDto>().ReverseMap();

            //Asignación
            CreateMap<TurnoAsignarMesaDto, Asignacion>();

        }
    }
}
