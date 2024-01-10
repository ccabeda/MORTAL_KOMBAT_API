using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MortalKombat_API.Models
{
    public class Personaje
    {
        [Key] //ponemos el id como key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Entity con sql server, para que aumente automaticamente el ID key.
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ImagenURl { get; set; }
        public string Alineacion { get; set; }
        public string Raza { get; set; }
        public string Descripcion { get; set; }
        public List<string> EstilosDePelea { get; set; }
        public int ClanId { set; get; }
        [ForeignKey("ClanId")]
        public Clan Clan { set; get; }
        public int ReinoId { set; get; }
        [ForeignKey("ReinoId")]
        public Reino Reino { set; get; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<Arma> Armas { get; set; }   
    }

}


