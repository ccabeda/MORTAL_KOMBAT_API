using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryLogin
    {
        Task<Usuario> Autenticar(Login entidad);
    }
}
