using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryUsuario : IRepositoryGeneric<Usuario>
    {
        private readonly AplicationDbContext _context;
        public RepositoryUsuario(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(Usuario user)
        {
            _context.Update(user);
        }

        public async Task Create(Usuario user)
        {
            await _context.Usuarios.AddAsync(user);
        }

        public async Task Delete(Usuario user)
        {
            _context.Remove(user);
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario?> GetByName(string username)
        {
            return await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.NombreDeUsuario == username);
        }

        public async Task<List<Usuario>> GetAll()
        {
            return await _context.Usuarios.AsNoTracking().Include(u => u.Rol).ToListAsync();
        }
    }
}
