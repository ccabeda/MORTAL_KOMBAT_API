namespace MortalKombat_API.Models.DTOs.PersonajeDTO
{
    public class PersonajeDto
    {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string ImagenURl { get; set; }
            public string Alineacion { get; set; }
            public string Raza { get; set; }
            public string Descripcion { get; set; }
            public List<string> EstilosDePelea { get; set; }
            public List<string> Armas { get; set; }
    }
}
