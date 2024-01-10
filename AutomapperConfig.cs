using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using AutoMapper;
using MortalKombat_API.Models;
using MortalKombat_API.Models.DTOs.ClanDTO;
using MortalKombat_API.Models.DTOs.PersonajeDTO;
using MortalKombat_API.Models.DTOs.ReinoDTO;

namespace MiPrimeraAPI
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

        }


    }
}
