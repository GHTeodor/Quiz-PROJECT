using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_PROJECT.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NickName { get; set; }
    
    [Column(TypeName = "nvarchar(24)")]
    public Role Role { get; set; } = Role.USER;
}