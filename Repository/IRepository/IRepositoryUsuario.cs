using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryUsuario : IRepositoryGeneric<Usuario>
    {
        Task<Usuario?> GetByMail(string mail);
    }
}
