using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await Task.FromResult<IActionResult>(Ok(await _questionService.Get()));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Question question)
    {
        var newQuestion = await _questionService.Post(question);

        return await Task.FromResult<IActionResult>(Accepted(newQuestion));
    }
}