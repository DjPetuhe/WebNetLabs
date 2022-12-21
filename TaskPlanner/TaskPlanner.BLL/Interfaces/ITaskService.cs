using System.Threading.Tasks;
using TaskPlanner.BLL.DTO.Task;
using System.Collections.Generic;

namespace TaskPlanner.BLL.Interfaces
{
    public interface ITaskService
    {
        public Task<TaskDto> GetById(int id);
        public Task<ICollection<TaskDto>> GetAll();
        public Task Delete(int id);
        public Task<TaskDto> Create(CreateTaskDto createTaskDto);
        public Task<TaskDto> Update(UpdateTaskDto updateTaskDto);
        public Task<TaskUsersDto> GetTaskUsersById(int id);
    }
}
