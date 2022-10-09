using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IAuthService
{
    Task<User> RegistrationAsync(CreateUserDTO user, CancellationToken token = default);
    Task<string> LoginAsync(AuthLoginUserDTO user, CancellationToken token = default);
    Task<string> RefreshTokenAsync(CancellationToken token = default);
    Task<Dictionary<string, string>> GetInfoFromTokenAsync(CancellationToken token = default);
    Task<User> UpdateByIdAsync(UpdateUserDTO user, long id, CancellationToken token = default);
    Task LogoutAsync(CancellationToken token = default);
    Task<List<User>> GetAllAsync(CancellationToken token = default);
}