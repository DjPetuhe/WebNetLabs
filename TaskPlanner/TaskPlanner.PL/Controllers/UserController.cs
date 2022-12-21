using System.Threading.Tasks;
using TaskPlanner.BLL.DTO.User;
using Microsoft.AspNetCore.Mvc;
using TaskPlanner.BLL.Interfaces;
using System.Collections.Generic;

namespace TaskPlanner.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<UserDto>>> Get()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            return Ok(await _userService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto user)
        {
            return Ok(await _userService.Create(user));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserDto user)
        {
            return Ok(await _userService.Update(user));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
        
        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<UserTasksDto>> GetTasksById(int id)
        {
            return Ok(await _userService.GetUserTasksById(id));
        }

        [HttpPut("{id}/tasks/{taskId}")]
        public async Task<ActionResult<UserTasksDto>> PutTaskById(int id, int taskId)
        {
            return Ok(await _userService.UpdateUserTaskById(id, taskId));
        }

        [HttpDelete("{id}/tasks/{taskId}")]
        public async Task<ActionResult<UserTasksDto>> DeleteTaskById(int id, int taskId)
        {
            return Ok(await _userService.DeleteUserTaskById(id, taskId));
        }

        [HttpGet("{id}/projects")]
        public async Task<ActionResult<UserProjectsDto>> GetProjectsById(int id)
        {
            return Ok(await _userService.GetUserProjectsById(id));
        }

        [HttpPut("{id}/projects/{projectId}")]
        public async Task<ActionResult<UserProjectsDto>> PutProjectById(int id, int projectId)
        {
            return Ok(await _userService.UpdateUserProjectById(id, projectId));
        }

        [HttpDelete("{id}/projects/{projectId}")]
        public async Task<ActionResult<UserTasksDto>> DeleteProjectById(int id, int projectId)
        {
            return Ok(await _userService.DeleteUserProjectById(id, projectId));
        }
    }
}
