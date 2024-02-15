using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryReino : IRepositoryReino
    {
        private readonly AplicationDbContext _context;
        public RepositoryReino(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reino?> ObtenerPorId(int id)
        {
             return await _context.Reinos.FindAsync(id);
        }

        public async Task<Reino?> ObtenerPorNombre(string name)
        {
            return await _context.Reinos.FirstOrDefaultAsync(r => r.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<Reino>> ObtenerTodos()
        {
            return await _context.Reinos.ToListAsync();
        }

        public async Task Crear(Reino reino)
        {
            await _context.Reinos.AddAsync(reino);
            await Guardar();
        }

        public async Task Eliminar(Reino reino)
        {
            _context.Reinos.Remove(reino);
            await Guardar();
        }

        public async Task Actualizar(Reino reino)
        {
            _context.Update(reino);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
