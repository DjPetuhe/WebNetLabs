using System.Threading.Tasks;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.DAL.Interfaces
{
    public interface ITaskRepository
    {
        public Task Add(Entities.Task task);
        public Task DeleteById(int taskID);
        public void Delete(Entities.Task task);
        public void Update(Entities.Task task);
        public Task<List<Entities.Task>> GetAll();
        public Task<Entities.Task?> GetById(int taskID);
    }
}
