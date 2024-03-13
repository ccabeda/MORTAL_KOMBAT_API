namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public record PersonajeDtoGetAll(int Id,
                           string Nombre,
                           string ImagenURL,
                           string Alineacion,
                           string Raza,
                           string Descripcion);
}
