using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryClan
    {
        Task<Clan?> ObtenerPorNombre(string nombre);
        Task<Clan?> ObtenerPorId(int id);
        Task<List<Clan>> ObtenerTodos();
        Task Crear(Clan clan);
        Task Actualizar(Clan clan);
        Task Eliminar(Clan clan);
        Task Guardar();
    }
}
