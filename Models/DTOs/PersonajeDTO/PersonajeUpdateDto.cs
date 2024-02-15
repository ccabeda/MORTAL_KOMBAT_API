namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public class PersonajeUpdateDto
    {
        public int Id { get; set; } 
        public required string Nombre { get; set; }
        public required string ImagenURl { get; set; }
        public required string Alineacion { get; set; }
        public required string Raza { get; set; }
        public required string Descripcion { get; set; }
        public int ClanId { set; get; }
        public int ReinoId { set; get; }
    }
}
