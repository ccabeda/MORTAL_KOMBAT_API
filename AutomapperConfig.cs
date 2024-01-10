using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using AutoMapper;


namespace API_MortalKombat
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Personaje, PersonajeDto>().ReverseMap();
            CreateMap<Personaje, PersonajeCreateDto>().ReverseMap();
            CreateMap<Personaje, PersonajeDtoGetAll>().ReverseMap();
            CreateMap<Personaje, PersonajeUpdateDto>().ReverseMap();
            CreateMap<Clan, ClanDto>().ReverseMap();
            CreateMap<Clan, ClanCreateDto>().ReverseMap();
            CreateMap<Clan, ClanUpdateDto>().ReverseMap();
            CreateMap<Reino, ReinoDto>().ReverseMap();
            CreateMap<Reino, ReinoCreateDto>().ReverseMap();
            CreateMap<Reino, ReinoUpdateDto>().ReverseMap();
            CreateMap<Arma, ArmaDto>().ReverseMap();
            CreateMap<Arma, ArmaCreateDto>().ReverseMap();
            CreateMap<Arma, ArmaUpdateDto>().ReverseMap();
            CreateMap<EstiloDePelea, EstiloDePeleaDto>().ReverseMap();
            CreateMap<EstiloDePelea, EstiloDePeleaCreateDto>().ReverseMap();
            CreateMap<EstiloDePelea, EstiloDePeleaUpdateDto>().ReverseMap();

        }


    }
}
