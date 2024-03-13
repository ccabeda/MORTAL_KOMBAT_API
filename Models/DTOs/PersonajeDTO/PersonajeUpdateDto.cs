namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{
    public record PersonajeUpdateDto(int Id,
                                     string Nombre,
                                     string ImagenURL,
                                     string Alineacion,
                                     string Raza,
                                     string Descripcion,
                                     int ClanId,
                                     int ReinoId);
}
