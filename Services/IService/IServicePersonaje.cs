using API_MortalKombat.Models.DTOs.PersonajeDTO;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;
using MortalKombat_API.Models.DTOs.PersonajeDTO;

namespace API_MortalKombat.Services.IServices
{
    public interface IServicePersonaje
    {
        public Task<APIResponse> GetPersonajes();
        public Task<APIResponse> GetPersonajeById(int id);
        public Task<APIResponse> GetPersonajeByName(String name);
        public Task<APIResponse> CreatePersonaje([FromBody] PersonajeCreateDto personajeCreateDto);
        public Task<APIResponse> PutPersonaje(int id, [FromBody] PersonajeUpdateDto personajeUpdateDto);
        public Task<APIResponse> DeletePersonaje(int id);

    }
}
