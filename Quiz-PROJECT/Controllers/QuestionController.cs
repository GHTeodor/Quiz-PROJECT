using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    private DBContext _dbContext;

    public QuestionController(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public Task<IActionResult> Get()
    {
        return Task.FromResult<IActionResult>(Ok(_dbContext.Question));
    }

    [HttpPost]
    public Task<IActionResult> Post([FromBody] Question questionX)
    {
        Console.WriteLine(questionX);

        _dbContext.Question.Add(questionX);
        _dbContext.SaveChangesAsync();

        return Task.FromResult<IActionResult>(Accepted(questionX));
    }
}