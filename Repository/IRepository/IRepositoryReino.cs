using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryReino
    {
        Task<Reino?> ObtenerPorNombre(string nombre);
        Task<Reino?> ObtenerPorId(int id);
        Task<List<Reino>> ObtenerTodos();
        Task Crear(Reino reino);
        Task Actualizar(Reino reino);
        Task Eliminar(Reino reino);
        Task Guardar();
    }
}
