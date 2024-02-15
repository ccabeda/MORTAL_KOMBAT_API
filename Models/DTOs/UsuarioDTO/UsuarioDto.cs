using API_MortalKombat.Models.DTOs.RolDTO;

namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Mail { get; set; }
        public required string NombreDeUsuario { get; set; }
        public RolDto? Rol { get; set; }
    }
}
