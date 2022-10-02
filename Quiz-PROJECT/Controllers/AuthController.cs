using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _authService.GetInfoFromTokenAsync());
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RegistrationAsync([FromBody] CreateUserDTO user)
    {
        return Accepted(await _authService.RegistrationAsync(user));
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthLoginUserDTO user)
    {
        return Ok(await _authService.LoginAsync(user));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> RefreshTokenAsync()
    {
        return Ok(await _authService.RefreshTokenAsync());
    }
    
    [HttpPost("[action]")]
    public async Task<ActionResult> LogoutAsync()
    {
        await _authService.LogoutAsync();
        return Ok();
    }
    
    [HttpPut("[action]/{id:long:min(1)}")]
    public async Task<IActionResult> UpdateByIdAsync([FromBody] UpdateUserDTO user, long id)
    {
        return Accepted(await _authService.UpdateByIdAsync(user, id));
    }
}