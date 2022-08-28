using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public Task<IActionResult> Get()
    {
        return Task.FromResult<IActionResult>(Ok(_userService.Get()));
    }
    
    [HttpGet("{id:int:min(1)}")]
    public Task<IActionResult> Get(int id)
    {
        return Task.FromResult<IActionResult>(Ok(_userService.GetById(id)));
    }

    [HttpPost]
    public Task<IActionResult> Post([FromBody] User person)
    {
        User newUser = _userService.Post(person);
        return Task.FromResult<IActionResult>(Accepted(newUser));
    }
    
    [HttpPatch("{id:int:min(1)}")]
    public Task<IActionResult> Patch([FromBody] User person, int id)
    {
        User updatedPerson = _userService.Put(person, id);
        return Task.FromResult<IActionResult>(Ok(updatedPerson));
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public Task<IActionResult> DeleteById(int id)
    {
        _userService.DeleteById(id);
        return Task.FromResult<IActionResult>(Ok(id));
    }
}