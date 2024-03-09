using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryEstiloDePelea
    {
        Task<EstiloDePelea?> GetByName(string name);
        Task<EstiloDePelea?> GetById(int id);
        Task<List<EstiloDePelea>> GetAll();
        Task Create(EstiloDePelea style);
        Task Update(EstiloDePelea style);
        Task Delete(EstiloDePelea style);
        Task Save();
    }
}
