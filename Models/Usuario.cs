using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_MortalKombat.Models
{
    public class Usuario
    {
        [Key] //ponemos el id como key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Mail { get; set; }
        public required string NombreDeUsuario { get; set; }
        public required string Contraseña { get; set; }
        public int RolId { set; get; }
        [ForeignKey("RolId")]
        public Rol Rol { set; get; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
