using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace API_MortalKombat.Repository
{
    public class RepositoryArma : IRepositoryArma
    {
        private readonly AplicationDbContext _context;
        public RepositoryArma(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Arma> ObtenerPorId(int id)
        {
            return await _context.Armas.FindAsync(id);
        }

        public async Task<Arma> ObtenerPorNombre(string name)
        {
            return await _context.Armas.FirstOrDefaultAsync(p => p.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<Arma>> ObtenerTodos()
        {
            return await _context.Armas.ToListAsync();
        }

        public async Task Crear(Arma arma)
        {
            await _context.Armas.AddAsync(arma);
            await Guardar();
        }

        public async Task Eliminar(Arma arma)
        {
            _context.Armas.Remove(arma);
            await Guardar();
        }

        public async Task Actualizar(Arma arma)
        {
            _context.Update(arma);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }
    }
}