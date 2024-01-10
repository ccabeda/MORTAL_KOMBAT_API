using Microsoft.AspNetCore.Mvc;
using MiPrimeraAPI.Models;
using MortalKombat_API.Models.DTOs.ClanDTO;

namespace API_MortalKombat.Services.IServices
{
    public interface IServiceClan
    {
        public Task<APIResponse> GetClanes();
        public Task<APIResponse> GetClanById(int id);
        public Task<APIResponse> GetClanByName(String name);
        public Task<APIResponse> CreateClan([FromBody] ClanCreateDto ClanCreateDto);
        public Task<APIResponse> UpdateClan(int id, [FromBody] ClanUpdateDto clanUpdateDto);
        public Task<APIResponse> DeleteClan(int id);
    }
}
