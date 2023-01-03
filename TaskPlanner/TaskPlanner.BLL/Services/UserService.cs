using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.BLL.Mappers;
using TaskPlanner.BLL.DTO.User;
using TaskPlanner.DAL.Entities;
using TaskPlanner.BLL.Exceptions;
using TaskPlanner.DAL.Interfaces;
using TaskPlanner.BLL.Interfaces;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> GetById(int id)
        {
            User user = await _unitOfWork.UserRepository.GetById(id) 
                              ?? throw new KeyNotFoundException($"User with id {id} was not found!");

            return Mapper.UserEntityToDto(user);
        }

        public async Task<ICollection<UserDto>> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            return users.Select(u => Mapper.UserEntityToDto(u)).ToList();
        }

        public async Task Delete(int id)
        {
            User user = await _unitOfWork.UserRepository.GetById(id)
                             ?? throw new KeyNotFoundException($"User with id {id} was not found!");

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDto> Create(CreateUserDto createUserDto)
        {
            if ((await _unitOfWork.UserRepository.GetByUserName(createUserDto.UserName)) is not null)
                throw new UserNameDuplicateException($"Username {createUserDto.UserName} already exists!");

            User user = new()
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                UserName = createUserDto.UserName,
                Passwords = createUserDto.Passwords
            };

            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.UserEntityToDto(user);
        }

        public async Task<UserDto> Update(UpdateUserDto updateUserDto)
        {
            User user = await _unitOfWork.UserRepository.GetById(updateUserDto.UserID) 
                              ?? throw new KeyNotFoundException($"User with id {updateUserDto.UserID} was not found!");

            User? userWithUsername = await _unitOfWork.UserRepository.GetByUserName(updateUserDto.UserName);
            if (userWithUsername is not null && userWithUsername.UserID != user.UserID) 
                throw new UserNameDuplicateException($"Username {updateUserDto.UserName} already exists!");

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.UserName = updateUserDto.UserName;
            user.Passwords = updateUserDto.Passwords;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.UserEntityToDto(user);
        }

        public async Task<UserTasksDto> GetUserTasksById(int id)
        {
            User user = await _unitOfWork.UserRepository.GetByIdWithTasks(id)
                              ?? throw new KeyNotFoundException($"User with id {id} was not found!");

            return Mapper.UserEntityToUserTaskDto(user);
        }

        public async Task<UserProjectsDto> GetUserProjectsById(int id)
        {
            User user = await _unitOfWork.UserRepository.GetByIdWithProjects(id)
                              ?? throw new KeyNotFoundException($"User with id {id} was not found!");

            return Mapper.UserEntityToUserProjectDto(user);
        }

        public async Task<UserTasksDto> UpdateUserTaskById(int userID, int taskID)
        {
            DAL.Entities.Task task = await _unitOfWork.TaskRepository.GetById(taskID)
                                           ?? throw new KeyNotFoundException($"Task with id {taskID} was not found!");

            User user = await _unitOfWork.UserRepository.GetByIdWithAll(userID)
                              ?? throw new KeyNotFoundException($"User with id {userID} was not found!");

            if (user.Projects is null || !user.Projects.Any(p => p.ProjectID == task.ProjectID))
                throw new UserAccesDeniedException($"User with id {userID} do not have acces to task with id {taskID}!");

            if (user.Tasks is not null && user.Tasks.Any(u => u.TaskID == taskID))
                throw new TaskAlreadyTakenException($"The user with id {userID} is already performing task with id {taskID}!");

            await _unitOfWork.UserRepository.AddToTask(userID, task);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.UserEntityToUserTaskDto(user);
        }

        public async Task<UserProjectsDto> UpdateUserProjectById(int userID, int projectID)
        {
            Project project = await _unitOfWork.ProjectRepository.GetById(projectID)
                                    ?? throw new KeyNotFoundException($"Project with id {projectID} was not found");

            User user = await _unitOfWork.UserRepository.GetByIdWithProjects(userID)
                              ?? throw new KeyNotFoundException($"User with id {userID} was not found!");

            if (user.Projects is not null && user.Projects.Any(u => u.ProjectID == projectID))
                throw new ProjectAlreadyTakenException($"User with id {userID} is already member of project with id {projectID}!");

            await _unitOfWork.UserRepository.AddToProject(userID, project);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.UserEntityToUserProjectDto(user);
        }

        public async Task<UserTasksDto> DeleteUserTaskById(int userID, int taskID)
        {
            DAL.Entities.Task task = await _unitOfWork.TaskRepository.GetById(taskID)
                                           ?? throw new KeyNotFoundException($"Task with id {taskID} was not found");

            User user = await _unitOfWork.UserRepository.GetByIdWithTasks(userID)
                              ?? throw new KeyNotFoundException($"User with id {userID} was not found!");

            if (user.Tasks is not null && !user.Tasks.Any(u => u.TaskID == taskID))
                throw new TaskNotTakenException($"User with id {userID} do not have a task with id {taskID} to delete!");

            await _unitOfWork.UserRepository.DeleteFromTask(userID, task);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.UserEntityToUserTaskDto(user);
        }

        public async Task<UserProjectsDto> DeleteUserProjectById(int userID, int projectID)
        {
            Project project = await _unitOfWork.ProjectRepository.GetById(projectID)
                                    ?? throw new KeyNotFoundException($"Project with id {projectID} was not found");

            User user = await _unitOfWork.UserRepository.GetByIdWithAll(userID)
                              ?? throw new KeyNotFoundException($"User with id {userID} was not found!");

            if (user.Projects is not null && !user.Projects.Any(u => u.ProjectID == projectID))
                throw new ProjectNotTakenException($"User with id {userID} is not a memeber of project with id {projectID}!");

            if (user.Tasks is not null)
            {
                var tasksOfProject = user.Tasks.Where(t => t.ProjectID == projectID);
                foreach (var task in tasksOfProject) await _unitOfWork.UserRepository.DeleteFromTask(userID, task);
            }

            await _unitOfWork.UserRepository.DeleteFromProject(userID, project);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.UserEntityToUserProjectDto(user);
        }
    }
}
