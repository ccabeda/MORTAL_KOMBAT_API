using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Models.DTOs.UsuarioDTO;
using Microsoft.AspNetCore.JsonPatch;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceUsuario 
    {
        public Task<APIResponse> GetAll();
        public Task<APIResponse> GetById(int id);
        public Task<APIResponse> GetByName(string name);
        public Task<APIResponse> CreateMyUsuario([FromBody] UsuarioCreateDto usuarioCreateDto);
        public Task<APIResponse> UpdateMyUsuario([FromBody] UsuarioUpdateDto usuarioUpdateDto, string username, string password);
        public Task<APIResponse> DeleteMyUsuario(string username, string password);
        public Task<APIResponse> UpdatePartialMyUsuario(JsonPatchDocument<UsuarioUpdateDto> usuarioUpdateDto, string username, string password);
        public Task<APIResponse> ADMIN_DeleteUsuario(int id);
    }
}