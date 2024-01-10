using API_MortalKombat.Models.DTOs.ArmaDTO;
using API_MortalKombat.Models.DTOs.ClanDTO;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;
using API_MortalKombat.Models.DTOs.ReinoDTO;

namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public class PersonajeDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenURl { get; set; }
        public string Alineacion { get; set; }
        public string Raza { get; set; }
        public string Descripcion { get; set; }
        public List<ArmaDto> Armas { get; set; }
        public List<EstiloDePeleaDto> EstilosDePeleas { get; set; }
        public ClanDto Clan { set; get; }
        public ReinoDto Reino { set; get; }
    }
}
