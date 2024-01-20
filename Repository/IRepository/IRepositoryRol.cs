using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryRol
    {
        Task<Rol> ObtenerPorNombre(string nombre);
        Task<Rol> ObtenerPorId(int id);
        Task<List<Rol>> ObtenerTodos();
        Task Crear(Rol rol);
        Task Actualizar(Rol rol);
        Task Eliminar(Rol rol);
        Task Guardar();
    }
}
