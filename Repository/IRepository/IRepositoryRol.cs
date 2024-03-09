using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryRol
    {
        Task<Rol?> GetByName(string name);
        Task<Rol?> GetById(int id);
        Task<List<Rol>> GetAll();
        Task Create(Rol rol);
        Task Update(Rol rol);
        Task Delete(Rol rol);
        Task Save();
    }
}
