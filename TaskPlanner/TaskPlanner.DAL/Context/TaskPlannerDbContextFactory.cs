using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskPlanner.DAL.Context
{
    internal class TaskPlannerDbContextFactory : IDesignTimeDbContextFactory<TaskPlannerDbContext>
    {
        public TaskPlannerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskPlannerDbContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=TaskPlannerDB;Trusted_Connection=true;TrustServerCertificate=true;");

            return new TaskPlannerDbContext(optionsBuilder.Options);
        }
    }
}