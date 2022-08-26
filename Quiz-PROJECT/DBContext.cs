using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}