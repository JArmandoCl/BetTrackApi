using AutoMapper;
using BetTrackApi.Dtos;
using BetTrackApi.Models;

namespace BetTrackApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Relaciones
            CreateMap<Usuario, DtoUsuario>().ReverseMap();
            CreateMap<RelApuesta, DtoApuesta>().ReverseMap();
            CreateMap<RelCategoriasUsuario, DtoCategoriaUsuario>().ReverseMap();
            CreateMap<RelDepositosRetiro, DtoDepositoRetiro>().ReverseMap();
            CreateMap<RelUsuarioBankroll, DtoUsuarioBankroll>().ReverseMap();
            CreateMap<RelUsuariosCasino, DtoUsuarioCasino>().ReverseMap();
            CreateMap<RelUsuarioTipster, DtoUsuarioTipster>().ReverseMap();
            //Catalogos
            CreateMap<EstatusUsuario, DtoEstatusUsuario>().ReverseMap();
            CreateMap<Casino, DtoCasino>().ReverseMap();
            CreateMap<EstatusUsuariosCasino, DtoEstatusUsuarioCasino>().ReverseMap();
            CreateMap<EstatusBankroll, DtoEstatusBankroll>().ReverseMap();
            CreateMap<FormatosCuota, DtoFormatoCuota>().ReverseMap();
            CreateMap<TiposBankroll, DtoTipoBankroll>().ReverseMap();
            CreateMap<EstatusCategoria, DtoEstatusCategoria>().ReverseMap();
            CreateMap<Deporte, DtoDeporte>().ReverseMap();
            CreateMap<EstatusApuesta, DtoEstatusApuesta>().ReverseMap();
            CreateMap<TiposApuesta, DtoTipoApuesta>().ReverseMap();
            CreateMap<Moneda, DtoMoneda>().ReverseMap();
        }
    }
}
