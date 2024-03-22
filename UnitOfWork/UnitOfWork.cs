using API_MortalKombat.Data;
using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;

namespace API_MortalKombat.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AplicationDbContext _context;
        public IRepositoryGeneric<Arma> repositoryArma { get; }
        public IRepositoryGeneric<Clan> repositoryClan { get; }
        public IRepositoryGeneric<EstiloDePelea> repositoryEstiloDePelea { get; }
        public IRepositoryGeneric<Reino> repositoryReino { get; }
        public IRepositoryGeneric<Rol> repositoryRol { get; }
        public IRepositoryGeneric<Personaje> repositoryPersonaje { get; }
        public IRepositoryUsuario repositoryUsuario { get; }

        public UnitOfWork(AplicationDbContext context, IRepositoryGeneric<Arma> _repositoryArma, IRepositoryGeneric<Clan> _repositoryClan, IRepositoryGeneric<EstiloDePelea> _repositoryEstiloDePelea,
                          IRepositoryGeneric<Reino> _repositoryReino, IRepositoryGeneric<Rol> _repositoryRol, IRepositoryGeneric<Personaje> _repositoryPersonaje, IRepositoryUsuario _repositoryUsuario)
        {
            _context = context;
            repositoryArma = _repositoryArma;
            repositoryClan = _repositoryClan;
            repositoryEstiloDePelea = _repositoryEstiloDePelea;
            repositoryReino = _repositoryReino;
            repositoryRol = _repositoryRol;
            repositoryPersonaje = _repositoryPersonaje;
            repositoryUsuario = _repositoryUsuario;
        }

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
