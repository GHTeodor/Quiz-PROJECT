using Microsoft.AspNetCore.Authentication.JwtBearer;
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

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken token = default)
    {
        return Ok(await _authService.GetAllAsync(token));
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAsync(CancellationToken token = default)
    {
        return Ok(await _authService.GetInfoFromTokenAsync(token));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RegistrationAsync([FromBody] CreateUserDTO user, CancellationToken token = default)
    {
        return Accepted(await _authService.RegistrationAsync(user, token));
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthLoginUserDTO user, CancellationToken token = default)
    {
        return Ok(await _authService.LoginAsync(user, token));
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> RefreshTokenAsync(CancellationToken token = default)
    {
        return Ok(await _authService.RefreshTokenAsync(token));
    }
    
    [HttpDelete("[action]")]
    public async Task<ActionResult> LogoutAsync(CancellationToken token = default)
    {
        await _authService.LogoutAsync(token);
        return Ok();
    }
    
    [HttpPut("[action]/{id:long:min(1)}")]
    public async Task<IActionResult> UpdateByIdAsync([FromBody] UpdateUserDTO user, long id, CancellationToken token = default)
    {
        return Accepted(await _authService.UpdateByIdAsync(user, id, token));
    }
}