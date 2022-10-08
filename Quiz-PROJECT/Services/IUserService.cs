using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken token = default);
    Task<User> GetByIdAsync(long id, CancellationToken token = default);
    Task DeleteByIdAsync(long id, CancellationToken token = default);
}