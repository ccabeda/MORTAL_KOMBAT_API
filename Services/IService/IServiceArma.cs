using API_MortalKombat.Models.DTOs.ArmaDTO;
using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceArma
    {
        public Task<APIResponse> GetArmas();
        public Task<APIResponse> GetArmaById(int id);
        public Task<APIResponse> GetArmaByName(String name);
        public Task<APIResponse> CreateArma([FromBody] ArmaCreateDto armaCreateDto);
        public Task<APIResponse> UpdateArma(int id, [FromBody] ArmaUpdateDto armaUpdateDto);
        public Task<APIResponse> DeleteArma(int id);
    }
}
