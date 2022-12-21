using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.BLL.Mappers;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.Interfaces;
using TaskPlanner.DAL.Interfaces;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskDto> GetById(int id)
        {
            var task = await _unitOfWork.TaskRepository.GetById(id)
                             ?? throw new KeyNotFoundException($"Task with id {id} was not found!");

            return Mapper.TaskEntityToDto(task);
        }

        public async Task<ICollection<TaskDto>> GetAll()
        {
            var tasks = await _unitOfWork.TaskRepository.GetAll();
            return tasks.Select(t => Mapper.TaskEntityToDto(t)).ToList();
        }

        public async Task Delete(int id)
        {
            var task = await _unitOfWork.TaskRepository.GetById(id)
                             ?? throw new KeyNotFoundException($"Task with id {id} was not found!");

            _unitOfWork.TaskRepository.Delete(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TaskDto> Create(CreateTaskDto createTaskDto)
        {
            if (await _unitOfWork.ProjectRepository.GetById(createTaskDto.ProjectID) is null)
                throw new KeyNotFoundException($"Project with id {createTaskDto.ProjectID} was not found!");

            DAL.Entities.Task task = new()
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Deadline = createTaskDto.Deadline,
                ProjectID = createTaskDto.ProjectID
            };

            await _unitOfWork.TaskRepository.Add(task);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.TaskEntityToDto(task);
        }

        public async Task<TaskDto> Update(UpdateTaskDto updateTaskDto)
        {
            var task = await _unitOfWork.TaskRepository.GetById(updateTaskDto.TaskID)
                             ?? throw new KeyNotFoundException($"Task with id {updateTaskDto.TaskID} was not found!");

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.Deadline = updateTaskDto.Deadline;

            _unitOfWork.TaskRepository.Update(task);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.TaskEntityToDto(task);
        }

        public async Task<TaskUsersDto> GetTaskUsersById(int id)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdWithUsers(id)
                             ?? throw new KeyNotFoundException($"Task with id {id} was not found");

            return Mapper.TaskEntityToTaskUsersDto(task);
        }
    }
}
