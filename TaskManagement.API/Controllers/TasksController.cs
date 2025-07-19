using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        // GET: api/<TasksController>
        [HttpGet("GetTaskAsync")]
        public async Task<IActionResult> Get()
        {
            var result = await _taskService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }

        // GET api/<TasksController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _taskService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // POST api/<TasksController>
        [HttpPost("SaveTask")]
        public async Task<IActionResult> Post([FromBody] TaskDto<int> taskDto)
        {
            var result = await _taskService.CreateAsync(taskDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<TasksController>/5
        [HttpPut("ModifyTask")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskDto<int> taskDto)
        {
            var result = await _taskService.UpdateAsync(id, taskDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<TasksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

    }
}
