namespace API_MortalKombat.Repository.IRepository
{
    public interface IRepositoryGeneric<T> where T : class
    {
        Task<T?> GetByName(string name);
        Task<T?> GetById(int id);
        Task<List<T>> GetAll();
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task Save();
    }
}

