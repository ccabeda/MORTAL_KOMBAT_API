using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryClan
    {
        Task<Clan?> GetByName(string name);
        Task<Clan?> GetById(int id);
        Task<List<Clan>> GetAll();
        Task Create(Clan clan);
        Task Update(Clan clan);
        Task Delete(Clan clan);
        Task Save();
    }
}
