using Microsoft.EntityFrameworkCore;
using LearningTodo.Api.Models;



namespace LearningTodo.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
        
}
