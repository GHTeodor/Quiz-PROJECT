using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;

namespace Quiz_PROJECT.Services;

public interface IAuthService
{
    Task<User> Register(CreateUserDTO user);
    Task<string> Login(AuthLoginUserDTO user);
    Task<string> RefreshToken();
    Task<Dictionary<string, string>> GetInfoFromToken();
}