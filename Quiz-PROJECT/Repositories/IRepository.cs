namespace Quiz_PROJECT.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteByIdAsync(int id);
}