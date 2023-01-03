using TaskPlanner.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace TaskPlanner.BLL.Tests
{
    public class TestDbContext : TaskPlannerDbContext
    {
        public TestDbContext(DbContextOptions<TaskPlannerDbContext> options) : base(options) { }
    }
}
