using MortalKombat_API.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryArma
    {
        Task<Arma> ObtenerPorNombre(string nombre);
        Task<Arma> ObtenerPorId(int id);
        Task<List<Arma>> ObtenerTodos();
        Task Crear(Arma arma);
        Task Actualizar(Arma arma);
        Task Eliminar(Arma arma);
        Task Guardar();
    }
}
