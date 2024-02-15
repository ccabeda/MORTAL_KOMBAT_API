using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryEstiloDePelea
    {
        Task<EstiloDePelea?> ObtenerPorNombre(string nombre);
        Task<EstiloDePelea?> ObtenerPorId(int id);
        Task<List<EstiloDePelea>> ObtenerTodos();
        Task Crear(EstiloDePelea estilo);
        Task Actualizar(EstiloDePelea estilo);
        Task Eliminar(EstiloDePelea estilo);
        Task Guardar();
    }
}
