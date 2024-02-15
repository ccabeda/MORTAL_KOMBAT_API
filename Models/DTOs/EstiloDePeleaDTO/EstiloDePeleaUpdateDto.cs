namespace API_MortalKombat.Models.DTOs.EstiloDePeleaDTO
{
    public class EstiloDePeleaUpdateDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
    }
}
