using TaskPlanner.DAL.Entities;
using TaskPlanner.BLL.DTO.User;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.DTO.Project;
using System.Linq;

namespace TaskPlanner.BLL.Mappers
{
    public static class Mapper
    {
        public static UserDto UserEntityToDto(User userEntity)
        {
            return new UserDto()
            {
                UserID = userEntity.UserID,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                UserName = userEntity.UserName,
                Passwords = userEntity.Passwords
            };
        }

        public static UserTasksDto UserEntityToUserTaskDto(User userEntity)
        {
            return new UserTasksDto()
            {
                UserID = userEntity.UserID,
                TasksID = userEntity.Tasks?.Select(t => t.TaskID)
            };
        }

        public static UserProjectsDto UserEntityToUserProjectDto(User userEntity)
        {
            return new UserProjectsDto()
            {
                UserID = userEntity.UserID,
                ProjectsID = userEntity.Projects?.Select(p => p.ProjectID)
            };
        }

        public static ProjectDto ProjectEntityToDto(Project projectEntity)
        {
            return new ProjectDto()
            {
                ProjectID = projectEntity.ProjectID,
                Name = projectEntity.Name
            };
        }

        public static ProjectUsersDto ProjectEntityToProjectUsersDto(Project projectEntity)
        {
            return new ProjectUsersDto()
            {
                ProjectID = projectEntity.ProjectID,
                UsersID = projectEntity.Users?.Select(u => u.UserID)
            };
        }

        public static ProjectTasksDto ProjectEntityToProjectTasksDto(Project projectEntity)
        {
            return new ProjectTasksDto()
            {
                ProjectID = projectEntity.ProjectID,
                TasksID = projectEntity.Tasks?.Select(t => t.TaskID)
            };
        }

        public static TaskDto TaskEntityToDto(Task taskEntity)
        {
            return new TaskDto()
            {
                TaskID = taskEntity.TaskID,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                Deadline = taskEntity.Deadline,
                ProjectID = taskEntity.ProjectID
            };
        }

        public static TaskUsersDto TaskEntityToTaskUsersDto(Task taskEntity)
        {
            return new TaskUsersDto()
            {
                TaskID = taskEntity.TaskID,
                UsersID = taskEntity.Users?.Select(u => u.UserID)
            };
        }
    }
}
