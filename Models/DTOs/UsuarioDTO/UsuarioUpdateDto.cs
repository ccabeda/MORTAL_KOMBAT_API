namespace API_MortalKombat.Models.DTOs.UsuarioDTO
{
    public class UsuarioUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public int RolId { get; set; }
    }
}
