using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Models;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            var token = await _authService.GenerateTokenAsync(loginRequest.Email, loginRequest.Password);

            if (token == null)
            {
                return Unauthorized(new { Message = "Credenciales invalidas" });
            }
            return Ok(new { Token = token });
        }
    }
}