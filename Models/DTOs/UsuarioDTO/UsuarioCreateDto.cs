namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public record UsuarioCreateDto(string Nombre,
                                   string Apellido,
                                   string Mail,
                                   string NombreDeUsuario,
                                   string Contraseña);
}
