using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryReino : IRepositoryGeneric<Reino>
    {
        private readonly AplicationDbContext _context;
        public RepositoryReino(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reino?> GetById(int id)
        {
             return await _context.Reinos.FindAsync(id);
        }

        public async Task<Reino?> GetByName(string name)
        {
            return await _context.Reinos.FirstOrDefaultAsync(r => r.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<Reino>> GetAll()
        {
            return await _context.Reinos.AsNoTracking().ToListAsync();
        }

        public async Task Create(Reino reino)
        {
            await _context.Reinos.AddAsync(reino);
            await Save();
        }

        public async Task Delete(Reino reino)
        {
            _context.Reinos.Remove(reino);
            await Save();
        }

        public async Task Update(Reino reino)
        {
            _context.Update(reino);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
