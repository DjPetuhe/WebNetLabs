using System.Threading.Tasks;
using TaskPlanner.DAL.Context;
using TaskPlanner.DAL.Interfaces;
using TaskPlanner.DAL.Repositories;

namespace TaskPlanner.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskPlannerDbContext _context;

        public UnitOfWork(TaskPlannerDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
            TaskRepository = new TaskRepository(context);
            ProjectRepository = new ProjectRepository(context);
        }

        public IUserRepository UserRepository { get; }

        public ITaskRepository TaskRepository { get; }

        public IProjectRepository ProjectRepository { get; }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}