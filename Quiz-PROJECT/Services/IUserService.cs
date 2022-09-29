using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync();
    Task<User> GetByIdAsync(long id);
    Task<User> UpdateByIdAsync(UpdateUserDTO user, long id);
    Task DeleteByIdAsync(long id);
}