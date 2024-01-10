using API_MortalKombat.Models;

namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryPersonaje
    {
        Task<Personaje> ObtenerPorNombre(string nombre);
        Task<Personaje> ObtenerPorId(int id);
        Task <List<Personaje>> ObtenerTodos ();
        Task Crear (Personaje personaje);
        Task<Arma> AgregarArmaAPersonaje(int id_arma);
        Task<EstiloDePelea> AgregarEstiloDePeleaAPersonaje(int id_estilo_de_pelea);
        Task Actualizar(Personaje personaje);
        Task Eliminar (Personaje personaje);
        Task Guardar();
    }
}
