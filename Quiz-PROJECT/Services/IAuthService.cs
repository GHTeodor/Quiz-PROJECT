using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(CreateUserDTO user);
    Task<string> LoginAsync(AuthLoginUserDTO user);
    Task<string> RefreshTokenAsync();
    Task<Dictionary<string, string>> GetInfoFromTokenAsync();
    Task<User> UpdateByIdAsync(UpdateUserDTO user, long id);
    Task LogoutAsync();
}