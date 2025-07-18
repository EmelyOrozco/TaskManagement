using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Persistence.Context
{
    public class TaskContext: DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
       public DbSet<Task> Tasks { get; set; }
       public DbSet<Users> Users { get; set; }
       public DbSet<Tags> Tags { get; set; }
       public DbSet<TasksTags> TasksTags { get; set; }
    
          

    }
}
