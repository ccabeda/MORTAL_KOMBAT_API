using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly AplicationDbContext _context;
        public RepositoryUsuario(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Actualizar(Usuario usuario)
        {
            _context.Update(usuario);
            await Guardar();
        }

        public async Task Crear(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await Guardar();
        }

        public async Task Eliminar(Usuario usuario)
        {
            _context.Remove(usuario);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> ObtenerPorNombre(string nombre)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.NombreDeUsuario == nombre);
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            return await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        }
    }
}
