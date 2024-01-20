using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryUsuario
    {
        Task<Usuario> ObtenerPorNombre(string nombre);
        Task<Usuario> ObtenerPorId(int id);
        Task<List<Usuario>> ObtenerTodos();
        Task Crear(Usuario usuario);
        Task Actualizar(Usuario usuario);
        Task Eliminar(Usuario usuario);
        Task Guardar();
    }
}
