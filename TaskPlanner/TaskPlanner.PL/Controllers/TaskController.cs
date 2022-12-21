using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.Interfaces;
using System.Collections.Generic;

namespace TaskPlanner.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<TaskDto>>> Get()
        {
            return Ok(await _taskService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            return Ok(await _taskService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto task)
        {
            return Ok(await _taskService.Create(task));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateTaskDto task)
        {
            return Ok(await _taskService.Update(task));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<TaskUsersDto>> GetUsersById(int id)
        {
            return Ok(await _taskService.GetTaskUsersById(id));
        }
    }
}
