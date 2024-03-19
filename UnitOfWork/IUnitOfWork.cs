using API_MortalKombat.Models;
using API_MortalKombat.Repository.IRepository;

namespace API_MortalKombat.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepositoryGeneric<Arma> repositoryArma { get; } //inserto los depositorios con get para poder ingresar desde el service
        public IRepositoryGeneric<Clan> repositoryClan { get; }
        public IRepositoryGeneric<EstiloDePelea> repositoryEstiloDePelea { get; }
        public IRepositoryGeneric<Reino> repositoryReino { get; }
        public IRepositoryGeneric<Rol> repositoryRol { get; }
        public IRepositoryGeneric<Personaje> repositoryPersonaje { get; }
        public IRepositoryGeneric<Usuario> repositoryUsuario { get; }

        Task Save();
    }
}
