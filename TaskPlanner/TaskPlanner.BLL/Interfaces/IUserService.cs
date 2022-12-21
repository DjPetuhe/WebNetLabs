using System.Threading.Tasks;
using TaskPlanner.BLL.DTO.User;
using System.Collections.Generic;

namespace TaskPlanner.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> GetById(int id);
        public Task<ICollection<UserDto>> GetAll();
        public Task Delete(int id);
        public Task<UserDto> Create(CreateUserDto createUserDto);
        public Task<UserDto> Update(UpdateUserDto updateUserDto);
        public Task<UserTasksDto> GetUserTasksById(int id);
        public Task<UserProjectsDto> GetUserProjectsById(int id);
        public Task<UserTasksDto> UpdateUserTaskById(int userID, int taskID);
        public Task<UserProjectsDto> UpdateUserProjectById(int userID, int projectID);
        public Task<UserTasksDto> DeleteUserTaskById(int userID, int taskID);
        public Task<UserProjectsDto> DeleteUserProjectById(int userID, int projectID);
    }
}
