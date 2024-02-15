namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public class PersonajeDtoGetAll
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Alineacion { get; set; }
        public required string ImagenURl { get; set; }
        public required string Raza { get; set; }
        public required string Descripcion { get; set; }
    }
}
