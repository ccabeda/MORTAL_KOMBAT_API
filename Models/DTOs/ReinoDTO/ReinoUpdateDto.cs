namespace API_MortalKombat.Models.DTOs.ReinoDTO
{
    public class ReinoUpdateDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
    }
}
