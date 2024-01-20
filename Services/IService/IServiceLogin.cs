using API_MortalKombat.Models;

namespace API_MortalKombat.Services.IService
{
    public interface IServiceLogin
    {
        public string GenerarTokendeLogin(Usuario usuario);
        public Task<APIResponse> LoginUsuario(Login login);
    }
}
