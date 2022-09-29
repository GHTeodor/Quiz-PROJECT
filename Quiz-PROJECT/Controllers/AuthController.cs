using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;
using Quiz_PROJECT.UnitOfWork;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        return Ok(await _authService.GetInfoFromToken());
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] CreateUserDTO user)
    {
        return Accepted(await _authService.Register(user));
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] AuthLoginUserDTO user)
    {
        return Accepted(await _authService.Login(user));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> RefreshToken()
    {
        return Ok(await _authService.RefreshToken());
    }
}