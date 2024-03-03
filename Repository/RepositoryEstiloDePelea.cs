using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryEstiloDePelea : IRepositoryEstiloDePelea
    {
        private readonly AplicationDbContext _context;
        public RepositoryEstiloDePelea(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EstiloDePelea?> ObtenerPorId(int id)
        {
            return await _context.EstilosDePeleas.FindAsync(id);
        }

        public async Task<EstiloDePelea?> ObtenerPorNombre(string name)
        {
            return await _context.EstilosDePeleas.FirstOrDefaultAsync(e => e.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<EstiloDePelea>> ObtenerTodos()
        {
            return await _context.EstilosDePeleas.AsNoTracking().ToListAsync();
        }

        public async Task Crear(EstiloDePelea estilo)
        {
            await _context.EstilosDePeleas.AddAsync(estilo);
            await Guardar();
        }

        public async Task Eliminar(EstiloDePelea estilo)
        {
            _context.EstilosDePeleas.Remove(estilo);
            await Guardar();
        }

        public async Task Actualizar(EstiloDePelea estilo)
        {
            _context.Update(estilo);
            await Guardar();
        }
        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }
    }
}
