using System.Threading.Tasks;
using TaskPlanner.DAL.Entities;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task Add(User user);
        public Task DeleteById(int userID);
        public void Delete(User user);
        public void Update(User user);
        public Task<List<User>> GetAll();
        public Task<User?> GetById(int userID);
        public Task<User?> GetByIdWithTasks(int userID);
        public Task<User?> GetByIdWithProjects(int userID);
        public Task<User?> GetByIdWithAll(int userID);
        public Task<User?> GetByUserName(string username);
        public Task AddToProject(int userID, Project project);
        public Task AddToTask(int userID, Entities.Task task);
        public Task DeleteFromProject(int userID, Project project);
        public Task DeleteFromTask(int userID, Entities.Task task);
    }
}
