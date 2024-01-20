using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_MortalKombat.Models
{
    public class Usuario
    {
        [Key] //ponemos el id como key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public int RolId { set; get; }
        [ForeignKey("RolId")]
        public Rol Rol { set; get; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

    }
}
