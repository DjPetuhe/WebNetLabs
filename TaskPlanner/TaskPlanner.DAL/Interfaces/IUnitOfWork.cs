using System.Threading.Tasks;

namespace TaskPlanner.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IProjectRepository ProjectRepository { get; }
        IUserRepository UserRepository { get; }
        ITaskRepository TaskRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
