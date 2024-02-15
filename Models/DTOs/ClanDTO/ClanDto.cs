namespace API_MortalKombat.Models.DTOs.ClanDTO
{
    public class ClanDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
    }
}
