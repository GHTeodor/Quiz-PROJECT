using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> FindByEmailAsync(string email, CancellationToken token = default);
    Task<User> FindByPhoneAsync(string phone, CancellationToken token = default);
}