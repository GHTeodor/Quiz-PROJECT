namespace Quiz_PROJECT.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken token = default);
    Task<T> GetByIdAsync(long id, CancellationToken token = default);
    Task CreateAsync(T item, CancellationToken token = default);
    Task UpdateAsync(T item);
    Task DeleteByIdAsync(long id, CancellationToken token = default);
}