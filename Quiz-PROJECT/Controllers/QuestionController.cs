using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models.DTOModels;
using Quiz_PROJECT.Services;

namespace Quiz_PROJECT.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
[Authorize]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _questionService.GetAllAsync());
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        return Ok(await _questionService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> PostCreateAsync([FromBody] CreateQuestionDTO question)
    {
        return Accepted(await _questionService.CreateAsync(question));
    }
    
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> PutUpdateByIdAsync([FromBody] UpdateQuestionDTO question, long id)
    {
        return Ok(await _questionService.UpdateByIdAsync(question, id));
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<IActionResult> DeleteByIdAsync(long id)
    {
        await _questionService.DeleteByIdAsync(id);
        return Ok(id);
    }
}