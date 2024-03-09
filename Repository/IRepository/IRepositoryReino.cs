using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryReino
    {
        Task<Reino?> GetByName(string name);
        Task<Reino?> GetById(int id);
        Task<List<Reino>> GetAll();
        Task Create(Reino reino);
        Task Update(Reino reino);
        Task Delete(Reino reino);
        Task Save();
    }
}
