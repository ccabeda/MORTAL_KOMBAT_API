using API_MortalKombat.Models.DTOs.RolDTO;

namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public record UsuarioDto(int Id,
                             string Nombre,
                             string Apellido,
                             string Mail,
                             string NombreDeUsuario,
                             RolDto? Rol);
}
