using System.Threading.Tasks;
using System.Collections.Generic;
using TaskPlanner.BLL.DTO.Project;

namespace TaskPlanner.BLL.Interfaces
{
    public interface IProjectService
    {
        public Task<ProjectDto> GetById(int id);
        public Task<ICollection<ProjectDto>> GetAll();
        public Task Delete(int id);
        public Task<ProjectDto> Create(CreateProjectDto createProjectDto);
        public Task<ProjectDto> Update(UpdateProjectDto updateProjectDto);
        public Task<ProjectTasksDto> GetProjectTasksById(int id);
        public Task<ProjectUsersDto> GetProjectUsersById(int id);
    }
}
