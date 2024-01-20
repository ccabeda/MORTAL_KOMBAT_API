using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ReinoDTO;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service.IService
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