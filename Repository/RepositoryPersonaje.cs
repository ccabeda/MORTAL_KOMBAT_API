using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryPersonaje : IRepositoryPersonaje
    {
        private readonly AplicationDbContext _context;
        public RepositoryPersonaje(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Personaje?> ObtenerPorId(int id)
        {
            return await _context.Personajes.Include(p => p.Clan).Include(p => p.Reino).Include(p => p.Armas).Include(p => p.EstilosDePeleas)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Personaje?> ObtenerPorNombre(string name)
        {
            return await _context.Personajes.Include(p => p.Clan).Include(p => p.Reino).Include(p => p.Armas).Include(p => p.EstilosDePeleas)
           .FirstOrDefaultAsync(p => p.Nombre.ToLower() == name.ToLower());
        }

        public async Task<List<Personaje>> ObtenerTodos()
        {
            return await _context.Personajes.ToListAsync();
        }

        public async Task Crear(Personaje personaje)
        {
            await _context.Personajes.AddAsync(personaje);
            await Guardar();
        }

        public async Task Eliminar(Personaje personaje)
        {
            _context.Personajes.Remove(personaje);
            await Guardar();
        }

        public async Task Actualizar(Personaje personaje)
        {
            _context.Update(personaje);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
