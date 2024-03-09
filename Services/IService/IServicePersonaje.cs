using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service.IService
{
    public interface IServicePersonaje
    {
        public Task<APIResponse> GetPersonajes();
        public Task<APIResponse> GetPersonajeById(int id);
        public Task<APIResponse> GetPersonajeByName(String name);
        public Task<APIResponse> CreatePersonaje([FromBody] PersonajeCreateDto personajeCreateDto);
        public Task<APIResponse> AddWeaponToPersonaje(int idPersonaje, int idArma);
        public Task<APIResponse> RemoveWeaponToPersonaje(int idPersonaje, int idArma);
        public Task<APIResponse> AddStyleToPersonaje(int idPersonaje, int idEstiloDePelea);
        public Task<APIResponse> RemoveStyleToPersonaje(int idPersonaje, int idEstiloDePelea);
        public Task<APIResponse> UpdatePersonaje(int id, [FromBody] PersonajeUpdateDto personajeUpdateDto);
        public Task<APIResponse> DeletePersonaje(int id);
        public Task<APIResponse> UpdatePartialPersonaje(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto);
    }
}