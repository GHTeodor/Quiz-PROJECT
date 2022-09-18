using Microsoft.EntityFrameworkCore;
using Quiz_PROJECT.Models;

namespace Quiz_PROJECT;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>()
            .HasMany(a => a.Answers)
            .WithOne(b => b.Question)
            .HasForeignKey(ab => ab.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Answer>()
             .HasOne(a => a.Question)
             .WithMany(b => b.Answers)
             .OnDelete(DeleteBehavior.NoAction);
             
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
}