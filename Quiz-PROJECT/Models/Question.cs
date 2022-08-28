using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_PROJECT.Models;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<Answers>? Answers { get; set; }
}