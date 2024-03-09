using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryLogin : IRepositoryLogin
    {
        private readonly AplicationDbContext _context;
        public RepositoryLogin(AplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario?> Authenticate(Login entity)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreDeUsuario == entity.Usuario && u.Contraseña == entity.Contraseña);
        }
    }
}
