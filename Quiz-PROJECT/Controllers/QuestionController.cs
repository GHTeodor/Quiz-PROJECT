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
    public async Task<IActionResult> Get()
    {
        return await Task.FromResult<IActionResult>(Ok(await _questionService.Get()));
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> Get(int id)
    {
        return await Task.FromResult<IActionResult>(Ok(await _questionService.GetById(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateQuestionDTO question)
    {
        Question newQuestion = await _questionService.Post(question);

        return await Task.FromResult<IActionResult>(Accepted(newQuestion));
    }
    
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromBody] UpdateQuestionDTO question, int id)
    {
        Question updatedQuestion = await _questionService.Put(question, id);
        return await Task.FromResult<IActionResult>(Ok(updatedQuestion));
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        await _questionService.DeleteById(id);
        return await Task.FromResult<IActionResult>(Ok(id));
    }
}