using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace API_MortalKombat.Repository
{
    public class RepositoryClan : IRepositoryClan
    {
        private readonly AplicationDbContext _context;
        public RepositoryClan(AplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Clan?> ObtenerPorId(int id)
        {
            return await _context.Clanes.FindAsync(id);
        }

        public async Task<Clan?> ObtenerPorNombre(string name)
        {
            return await _context.Clanes.FirstOrDefaultAsync(c => c.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<Clan>> ObtenerTodos()
        {
            return await _context.Clanes.AsNoTracking().ToListAsync();
        }

        public async Task Crear(Clan clan)
        {
            await _context.Clanes.AddAsync(clan);
            await Guardar();
        }

        public async Task Eliminar(Clan clan)
        {
            _context.Clanes.Remove(clan);
            await Guardar();
        }

        public async Task Actualizar(Clan clan)
        {
            _context.Update(clan);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
