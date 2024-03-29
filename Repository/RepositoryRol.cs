﻿using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryRol : IRepositoryGeneric<Rol>
    {
        private readonly AplicationDbContext _context;
        public RepositoryRol(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task Update(Rol rol) => _context.Update(rol);
        
        public async Task Create(Rol rol) => await _context.Roles.AddAsync(rol);
        
        public async Task Delete(Rol rol) => _context.Roles.Remove(rol);
        
        public async Task<Rol?> GetById(int id) => await _context.Roles.FindAsync(id);
        
        public async Task<Rol?> GetByName(string nombre) => await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
        
        public async Task<List<Rol>> GetAll() => await _context.Roles.AsNoTracking().ToListAsync();
    }
}
