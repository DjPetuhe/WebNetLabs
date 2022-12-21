using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.BLL.Mappers;
using TaskPlanner.DAL.Entities;
using TaskPlanner.BLL.Interfaces;
using TaskPlanner.DAL.Interfaces;
using System.Collections.Generic;
using TaskPlanner.BLL.DTO.Project;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDto> GetById(int id)
        {
            Project project = await _unitOfWork.ProjectRepository.GetById(id)
                                    ?? throw new KeyNotFoundException($"Project with id {id} was not found!");

            return Mapper.ProjectEntityToDto(project);
        }

        public async Task<ICollection<ProjectDto>> GetAll()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAll();
            return projects.Select(p => Mapper.ProjectEntityToDto(p)).ToList();
        }

        public async Task Delete(int id)
        {
            Project project = await _unitOfWork.ProjectRepository.GetById(id)
                                    ?? throw new KeyNotFoundException($"Project with id {id} was not found!");

            _unitOfWork.ProjectRepository.Delete(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ProjectDto> Create(CreateProjectDto createProjectDto)
        {
            Project project = new()
            {
                Name = createProjectDto.Name
            };

            await _unitOfWork.ProjectRepository.Add(project);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.ProjectEntityToDto(project);
        }

        public async Task<ProjectDto> Update(UpdateProjectDto updateProjectDto)
        {
            Project project = await _unitOfWork.ProjectRepository.GetById(updateProjectDto.ProjectID)
                                    ?? throw new KeyNotFoundException($"Project with id {updateProjectDto.ProjectID} was not found!");

            project.Name = updateProjectDto.Name;

            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangesAsync();
            return Mapper.ProjectEntityToDto(project);
        }

        public async Task<ProjectTasksDto> GetProjectTasksById(int id)
        {
            Project project = await _unitOfWork.ProjectRepository.GetByIdWithTasks(id)
                                    ?? throw new KeyNotFoundException($"Project with id {id} was not found!");

            return Mapper.ProjectEntityToProjectTasksDto(project);
        }

        public async Task<ProjectUsersDto> GetProjectUsersById(int id)
        {
            Project project = await _unitOfWork.ProjectRepository.GetByIdWithUsers(id)
                                    ?? throw new KeyNotFoundException($"Project with id {id} was not found!");

            return Mapper.ProjectEntityToProjectUsersDto(project);
        }
    }
}
