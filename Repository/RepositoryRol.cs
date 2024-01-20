using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryRol : IRepositoryRol
    {
        private readonly AplicationDbContext _context;
        public RepositoryRol(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Actualizar(Rol rol)
        {
            _context.Update(rol);
            await Guardar();
        }

        public async Task Crear(Rol rol)
        {
            await _context.Roles.AddAsync(rol);
            await Guardar();
        }

        public async Task Eliminar(Rol rol)
        {
            _context.Roles.Remove(rol);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();  
        }

        public async Task<Rol> ObtenerPorId(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Rol> ObtenerPorNombre(string nombre)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task<List<Rol>> ObtenerTodos()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
