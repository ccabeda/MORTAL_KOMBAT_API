using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MortalKombat_API.Models.DTOs.PersonajeDTO
{
    public class PersonajeCreateDto
    {

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
