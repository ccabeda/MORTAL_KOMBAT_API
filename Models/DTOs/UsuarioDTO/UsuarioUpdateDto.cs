namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public record UsuarioUpdateDto(string Nombre, //sin el ID, ya que el usuario no lo necesita saber para modificar sus datos
                                   string Apellido,
                                   string Mail,
                                   string NombreDeUsuario,
                                   string Contraseña);
}
