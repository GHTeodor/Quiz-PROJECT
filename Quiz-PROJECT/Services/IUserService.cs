using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> Get();
    Task<User> GetById(int id);
    Task<User> Post(CreateUserDTO user);
    Task<User> Put(UpdateUserDTO user, int id);
    Task DeleteById(int id);
}