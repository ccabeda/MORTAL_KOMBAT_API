using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryEstiloDePelea : IRepositoryGeneric<EstiloDePelea>
    {
        private readonly AplicationDbContext _context;
        public RepositoryEstiloDePelea(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EstiloDePelea?> GetById(int id)
        {
            return await _context.EstilosDePeleas.FindAsync(id);
        }

        public async Task<EstiloDePelea?> GetByName(string name)
        {
            return await _context.EstilosDePeleas.FirstOrDefaultAsync(e => e.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<EstiloDePelea>> GetAll()
        {
            return await _context.EstilosDePeleas.AsNoTracking().ToListAsync();
        }

        public async Task Create(EstiloDePelea style)
        {
            await _context.EstilosDePeleas.AddAsync(style);
        }

        public async Task Delete(EstiloDePelea style)
        {
            _context.EstilosDePeleas.Remove(style);
        }

        public async Task Update(EstiloDePelea style)
        {
            _context.Update(style);
        }
    }
}
