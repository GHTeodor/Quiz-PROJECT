using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quiz_PROJECT.Models;
using Quiz_PROJECT.Models.DTOModels;
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
    public async Task<IActionResult> GetAllAsync()
    {
        return await Task.FromResult<IActionResult>(Ok(await _questionService.GetAllAsync()));
    }
    
    [HttpGet("{id:long:min(1)}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        return await Task.FromResult<IActionResult>(Ok(await _questionService.GetByIdAsync(id)));
    }

    [HttpPost]
    public async Task<IActionResult> PostCreateAsync([FromBody] CreateQuestionDTO question)
    {
        Question newQuestion = await _questionService.CreateAsync(question);
        return await Task.FromResult<IActionResult>(Accepted(newQuestion));
    }
    
    [HttpPut("{id:long:min(1)}")]
    public async Task<IActionResult> PutUpdateByIdAsync([FromBody] UpdateQuestionDTO question, long id)
    {
        Question updatedQuestion = await _questionService.UpdateByIdAsync(question, id);
        return await Task.FromResult<IActionResult>(Ok(updatedQuestion));
    }
    
    [HttpDelete("{id:long:min(1)}")]
    public async Task<IActionResult> DeleteByIdAsync(long id)
    {
        await _questionService.DeleteByIdAsync(id);
        return await Task.FromResult<IActionResult>(Ok(id));
    }
}