﻿using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace API_MortalKombat.Repository
{
    public class RepositoryArma : IRepositoryGeneric<Arma>
    {
        private readonly AplicationDbContext _context;
        public RepositoryArma(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Arma?> GetById(int id) => await _context.Armas.FindAsync(id); //aqui no uso asNoTracking ya que voy a usarlo para actualizarlo 
        
        public async Task<Arma?> GetByName(string name) => await _context.Armas.FirstOrDefaultAsync(a => a.Nombre.ToLower() == name.ToLower()); //como solo quiero mostrar el nombre, utilizo AsNoTracking
        
        public async Task<List<Arma>> GetAll() =>  await _context.Armas.AsNoTracking().ToListAsync(); //como solo quiero mostrar el nombre, utilizo AsNoTracking
        
        public async Task Create(Arma arm) => await _context.Armas.AddAsync(arm);
        
        public async Task Delete(Arma arm) => _context.Armas.Remove(arm);
        
        public async Task Update(Arma arm) => _context.Update(arm);        
    }
}