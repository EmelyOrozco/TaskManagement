using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        // GET: api/<UsersController>
        [HttpGet("GetUsers")]
        public async Task<IActionResult> Get()
        {
            var result = await _usersService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }

        // GET api/<UsersController>/5
        [HttpGet("GetUsersByid{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _usersService.GetByIdAsync(id);

            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<UsersController>
        [HttpPost("SaveUsers")]
        public async Task<IActionResult> Post([FromBody] UsersDto<int> usersDto)
        {
            var result = await _usersService.CreateAsync(usersDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<UsersController>/5
        [HttpPut("UpdateUsers{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsersDto<int> usersDto)
        {
            var result = await _usersService.UpdateAsync(id, usersDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("DeleteUsers{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usersService.DeleteAsync(id);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
