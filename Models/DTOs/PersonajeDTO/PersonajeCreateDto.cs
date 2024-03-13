namespace API_MortalKombat.Models.DTOs.PersonajeDTO
{ 
    public record PersonajeCreateDto(string Nombre,
                                     string ImagenURL,
                                     string Alineacion,
                                     string Raza,
                                     string Descripcion,
                                     int ClanId,
                                     int ReinoId);
}
