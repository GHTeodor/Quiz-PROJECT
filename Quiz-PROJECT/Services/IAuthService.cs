using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IAuthService
{
    Task<User> RegistrationAsync(CreateUserDTO user, CancellationToken token = default);
    Task<Login_Refresh_JWTResponseDTO> LoginAsync(AuthLoginUserDTO user, CancellationToken token = default);
    Task<Login_Refresh_JWTResponseDTO> RefreshTokenAsync(CancellationToken token = default);
    Task<Dictionary<string, string>> GetInfoFromTokenAsync();
    Task LogoutAsync(CancellationToken token = default);
    Task<List<User>> GetAllAsync(CancellationToken token = default);
}