using API_MortalKombat.Models.DTOs.RolDTO;
using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceRol
    {
        public Task<APIResponse> GetRoles();
        public Task<APIResponse> GetRolById(int id);
        public Task<APIResponse> GetRolByName(String name);
        public Task<APIResponse> CreateRol([FromBody] RolCreateDto rolCreateDto);
        public Task<APIResponse> UpdateRol(int id, [FromBody] RolUpdateDto rolUpdateDto);
        public Task<APIResponse> DeleteRol(int id);
    }
}