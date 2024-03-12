namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public class UsuarioUpdateDto
    {       
        //sin el ID, ya que el usuario no lo necesita saber para modificar sus datos
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Mail { get; set; }
        public required string NombreDeUsuario { get; set; }
        public required string Contraseña { get; set; }
    }
}
