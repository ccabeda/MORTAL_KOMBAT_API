using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryUsuario
    {
        Task<Usuario?> GetByName(string name);
        Task<Usuario?> GetById(int id);
        Task<List<Usuario>> GetAll();
        Task Create(Usuario user);
        Task Update(Usuario user);
        Task Delete(Usuario user);
        Task Save();
    }
}
