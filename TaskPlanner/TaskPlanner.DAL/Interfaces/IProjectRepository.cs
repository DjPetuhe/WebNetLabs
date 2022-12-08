using System.Threading.Tasks;
using TaskPlanner.DAL.Entities;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.DAL.Interfaces
{
    public interface IProjectRepository
    {
        public Task Add(Project project);
        public Task DeleteById(int projectID);
        public void Delete(Project project);
        public void Update(Project project);
        public Task<List<Project>> GetAll();
        public Task<Project?> GetById(int projectID);
    }
}
