using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Services;

public interface IUserService
{
    public IEnumerable<User> Get();
    public User GetById(int id);
    public User Post(User user);
    public User Put(User user, int id);
    public void DeleteById(int id);
}