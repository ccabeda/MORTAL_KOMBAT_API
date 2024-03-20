using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace API_MortalKombat.Repository
{
    public class RepositoryClan : IRepositoryGeneric<Clan>
    {
        private readonly AplicationDbContext _context;
        public RepositoryClan(AplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Clan?> GetById(int id)
        {
            return await _context.Clanes.FindAsync(id);
        }

        public async Task<Clan?> GetByName(string name)
        {
            return await _context.Clanes.FirstOrDefaultAsync(c => c.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<Clan>> GetAll()
        {
            return await _context.Clanes.AsNoTracking().ToListAsync();
        }

        public async Task Create(Clan clan)
        {
            await _context.Clanes.AddAsync(clan);
        }

        public async Task Delete(Clan clan)
        {
            _context.Clanes.Remove(clan);
        }

        public async Task Update(Clan clan)
        {
            _context.Update(clan);
        }
    }
}
