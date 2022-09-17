using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await Task.FromResult<IActionResult>(Ok(_userService.Get()));
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> Get(int id)
    {
        return await Task.FromResult<IActionResult>(Ok(await _userService.GetById(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserDTO person)
    {
        User newUser = await _userService.Post(person);
        return await Task.FromResult<IActionResult>(Accepted(newUser));
    }
    
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromBody] UpdateUserDTO person, int id)
    {
        User updatedPerson = await _userService.Put(person, id);
        return await Task.FromResult<IActionResult>(Ok(updatedPerson));
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var resId = await _userService.DeleteById(id);
        return await Task.FromResult<IActionResult>(Ok(resId));
    }
}