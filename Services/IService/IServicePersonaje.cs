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
        public Task<APIResponse> AddWeaponToPersonaje(int id_personaje, int id_arma);
        public Task<APIResponse> RemoveWeaponToPersonaje(int id_personaje, int id_arma);
        public Task<APIResponse> AddStyleToPersonaje(int id_personaje, int id_estilo_de_pelea);
        public Task<APIResponse> RemoveStyleToPersonaje(int id_personaje, int id_estilo_de_pelea);
        public Task<APIResponse> UpdatePersonaje(int id, [FromBody] PersonajeUpdateDto personajeUpdateDto);
        public Task<APIResponse> DeletePersonaje(int id);
        public Task<APIResponse> UpdatePartialPersonaje(int id, JsonPatchDocument<PersonajeUpdateDto> personajeUpdateDto);
    }
}