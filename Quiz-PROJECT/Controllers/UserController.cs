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
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _userService.GetAllAsync());
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpDelete("{id:long:min(1)}")]
    public async Task<IActionResult> DeleteByIdAsync(long id)
    {
        await _userService.DeleteByIdAsync(id);
        return Ok(id);
    }
}