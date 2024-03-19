using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.PersonajeDTO;


namespace API_MortalKombat.Services.IService
{
    public interface IServicePersonaje : IServiceGeneric<PersonajeUpdateDto, PersonajeCreateDto>
    {
        public Task<APIResponse> AddWeapon(int idPersonaje, int idArma);
        public Task<APIResponse> RemoveWeapon(int idPersonaje, int idArma);
        public Task<APIResponse> AddStyle(int idPersonaje, int idEstiloDePelea);
        public Task<APIResponse> RemoveStyle(int idPersonaje, int idEstiloDePelea);
    }
}