namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public class UsuarioUpdateDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Mail { get; set; }
        public required string NombreDeUsuario { get; set; }
        public required string Contraseña { get; set; }
        public int RolId { get; set; }
    }
}
