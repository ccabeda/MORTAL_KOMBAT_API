using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using API_MortalKombat.Models.DTOs.ReinoDTO;

namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public record PersonajeDto(int Id,
                               string Nombre,
                               string ImagenURL,
                               string Alineacion,
                               string Raza,
                               string Descripcion,
                               List<ArmaDto>? Armas,
                               List<EstiloDePeleaDto> EstilosDePeleas,
                               ClanDto? Clan,
                               ReinoDto? Reino);
}
