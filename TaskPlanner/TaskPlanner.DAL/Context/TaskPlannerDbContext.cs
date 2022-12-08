using TaskPlanner.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Task = TaskPlanner.DAL.Entities.Task;

namespace TaskPlanner.DAL.Context
{
    public class TaskPlannerDbContext : DbContext
    {
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public TaskPlannerDbContext(DbContextOptions<TaskPlannerDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}