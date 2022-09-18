using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> FindByEmailAsync(string email);
    Task<User> FindByPhoneAsync(string phone);
}