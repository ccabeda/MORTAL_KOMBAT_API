using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryPersonaje
    {
        Task<Personaje?> GetByName(string name);
        Task<Personaje?> GetById(int id);
        Task <List<Personaje>> GetAll();
        Task Create(Personaje character);
        Task Update(Personaje character);
        Task Delete(Personaje character);
        Task Save();
    }
}
