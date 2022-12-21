using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskPlanner.BLL.Interfaces;
using TaskPlanner.BLL.DTO.Project;

namespace TaskPlanner.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ProjectDto>>> Get()
        {
            return Ok(await _projectService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            return Ok(await _projectService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto project)
        {
            return Ok(await _projectService.Create(project));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProjectDto project)
        {
            return Ok(await _projectService.Update(project));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<ProjectTasksDto>> GetTasksById(int id)
        {
            return Ok(await _projectService.GetProjectTasksById(id));
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<ProjectUsersDto>> GetUsersById(int id)
        {
            return Ok(await _projectService.GetProjectUsersById(id));
        }
    }
}
