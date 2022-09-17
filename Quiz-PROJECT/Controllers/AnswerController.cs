using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;

    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await Task.FromResult<IActionResult>(Ok(await _answerService.Get()));
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Answers answer)
    {
        var newAnswer = await _answerService.Post(answer);

        return await Task.FromResult<IActionResult>(Accepted(newAnswer));
    }
}