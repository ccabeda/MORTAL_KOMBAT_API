using API_MortalKombat.Models;
using Microsoft.AspNetCore.Mvc;
using API_MortalKombat.Models.DTOs.UsuarioDTO;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceUsuario
    {
        public Task<APIResponse> GetUsuarios();
        public Task<APIResponse> GetUsuarioById(int id);
        public Task<APIResponse> GetUsuarioByName(String name);
        public Task<APIResponse> CreateUsuario([FromBody] UsuarioCreateDto usuarioCreateDto);
        public Task<APIResponse> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto usuarioUpdateDto);
        public Task<APIResponse> DeleteUsuario(int id);
    }
}