using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;
using MortalKombat_API.Models.DTOs.ReinoDTO;

namespace API_MortalKombat.Services.IServices
{
    public interface IServiceReino
    {
        public Task<APIResponse> GetReinos();
        public Task<APIResponse> GetReinoById(int id);
        public Task<APIResponse> GetReinoByName(String name);
        public Task<APIResponse> CreateReino([FromBody] ReinoCreateDto reinoCreateDto);
        public Task<APIResponse> UpdateReino(int id, [FromBody] ReinoUpdateDto reinoUpdateDto);
        public Task<APIResponse> DeleteReino(int id);
    }
}
