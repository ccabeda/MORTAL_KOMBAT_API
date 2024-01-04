using MortalKombat_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public class PersonajeUpdateDto
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public string ImagenURl { get; set; }
        public string Alineacion { get; set; }
        public string Raza { get; set; }
        public string Descripcion { get; set; }
        public List<string> EstilosDePelea { get; set; }
        public List<string> Armas { get; set; }
        public int ClanId { set; get; }
        public int ReinoId { set; get; }
    }
}
