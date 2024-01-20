using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Models.DTOs.EstiloDePeleaDTO;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceEstiloDePelea
    {
        public Task<APIResponse> GetEstilosDePelea();
        public Task<APIResponse> GetEstiloDePeleaById(int id);
        public Task<APIResponse> GetEstiloDePeleaByName(String name);
        public Task<APIResponse> CreateEstiloDePelea([FromBody] EstiloDePeleaCreateDto estiloCreateDto);
        public Task<APIResponse> UpdateEstiloDePelea(int id, [FromBody] EstiloDePeleaUpdateDto estiloUpdateDto);
        public Task<APIResponse> DeleteEstiloDePelea(int id);
    }
}