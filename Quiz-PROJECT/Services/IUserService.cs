using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public interface IUserService
{
    public IEnumerable<User> Get();
    public User GetById(int id);
    public Task<User> Post(User user);
    public Task<User> Put(User user, int id);
    public void DeleteById(int id);
}