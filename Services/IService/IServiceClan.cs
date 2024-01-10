using API_MortalKombat.Models;
using API_MortalKombat.Models.DTOs.ClanDTO;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Service.IService
{
    public interface IServiceClan
    {
        public Task<APIResponse> GetClanes();
        public Task<APIResponse> GetClanById(int id);
        public Task<APIResponse> GetClanByName(String name);
        public Task<APIResponse> CreateClan([FromBody] ClanCreateDto clanCreateDto);
        public Task<APIResponse> UpdateClan(int id, [FromBody] ClanUpdateDto clanUpdateDto);
        public Task<APIResponse> DeleteClan(int id);
    }
}
