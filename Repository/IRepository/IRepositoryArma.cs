using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryArma
    {
        Task<Arma?> GetByName(string name);
        Task<Arma?> GetById(int id);
        Task<List<Arma>> GetAll();
        Task Create(Arma arm);
        Task Update(Arma arm);
        Task Delete(Arma arm);
        Task Save();
    }
}
