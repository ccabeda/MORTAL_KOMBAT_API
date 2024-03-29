﻿using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryPersonaje : IRepositoryGeneric<Personaje>
    {
        private readonly AplicationDbContext _context;
        public RepositoryPersonaje(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Personaje?> GetById(int id) => await _context.Personajes.Include(p => p.Clan).Include(p => p.Reino)
                                                        .Include(p => p.Armas).Include(p => p.EstilosDePeleas).FirstOrDefaultAsync(p => p.Id == id);
        
        public async Task<Personaje?> GetByName(string name) => await _context.Personajes.Include(p => p.Clan).Include(p => p.Reino)
                                                               .Include(p => p.Armas).Include(p => p.EstilosDePeleas).FirstOrDefaultAsync(p => p.Nombre.ToLower() == name.ToLower());
        
        public async Task<List<Personaje>> GetAll() => await _context.Personajes.AsNoTracking().ToListAsync();
        
        public async Task Create(Personaje character) => await _context.Personajes.AddAsync(character);
        
        public async Task Delete(Personaje character) => _context.Personajes.Remove(character);
        
        public async Task Update(Personaje character) => _context.Update(character);
    }
}
