using API_MortalKombat.Models.DTOs.RolDTO;

namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string NombreDeUsuario { get; set; }
        public RolDto Rol { get; set; }
    }
}
