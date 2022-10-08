using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken token = default)
    {
        return Ok(await _userService.GetAllAsync(token));
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<IActionResult> GetByIdAsync(long id, CancellationToken token = default)
    {
        return Ok(await _userService.GetByIdAsync(id, token));
    }

    [HttpDelete("{id:long:min(1)}")]
    public async Task<IActionResult> DeleteByIdAsync(long id, CancellationToken token = default)
    {
        await _userService.DeleteByIdAsync(id, token);
        return Ok(id);
    }
}