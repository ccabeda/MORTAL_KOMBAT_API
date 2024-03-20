using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_MortalKombat.Models
{
    public class Personaje
    {
        [Key] //ponemos el id como key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Entity con sql server, para que aumente automaticamente el ID key.
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? ImagenURl { get; set; }
        public required string Alineacion { get; set; }
        public required string Raza { get; set; }
        public required string Descripcion { get; set; }
        public int ClanId { set; get; }
        [ForeignKey("ClanId")]
        public Clan? Clan { set; get; }
        public int ReinoId { set; get; }
        [ForeignKey("ReinoId")]
        public Reino? Reino { set; get; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<Arma>? Armas { get; set; }
        public List<EstiloDePelea>? EstilosDePeleas { get; set; } 
    }

}


