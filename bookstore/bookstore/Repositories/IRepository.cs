namespace bookstore.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity obj);
        Task EditAsync(TEntity obj);
        Task DeleteAsync(int id);
    }
}
