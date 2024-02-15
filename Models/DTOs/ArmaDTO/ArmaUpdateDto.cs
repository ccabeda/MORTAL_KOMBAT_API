namespace API_MortalKombat.Models.DTOs.ArmaDTO
{
    public class ArmaUpdateDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
    }
}
