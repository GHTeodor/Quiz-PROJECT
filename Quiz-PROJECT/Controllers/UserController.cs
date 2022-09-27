using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
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
        return await Task.FromResult<IActionResult>(Ok(await _userService.GetAllAsync()));
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        return await Task.FromResult<IActionResult>(Ok(await _userService.GetByIdAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> PostCreateAsync([FromBody] CreateUserDTO person)
    {
        User newUser = await _userService.CreateAsync(person);
        return await Task.FromResult<IActionResult>(Accepted(newUser));
    }
    
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> PutUpdateByIdAsync([FromBody] UpdateUserDTO person, int id)
    {
        User updatedPerson = await _userService.UpdateByIdAsync(person, id);
        return await Task.FromResult<IActionResult>(Ok(updatedPerson));
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<IActionResult> DeleteByIdAsync(long id)
    {
        await _userService.DeleteByIdAsync(id);
        return await Task.FromResult<IActionResult>(Ok(id));
    }
}