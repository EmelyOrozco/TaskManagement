using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Models;
using TaskManagement.Application.Interfaces.Services;

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

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });
            return Ok(new { Token = token });
        }
        [Authorize]
        [HttpPost("Validation")]
        public IActionResult Validate()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var email = User.FindFirst("Email")?.Value;
            var username = User.FindFirst("UserName")?.Value;

            return Ok(new { UserId = userId, Email = email, UserName = username });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { Message = "Cierre de sesión exitoso" });
        }

    }
}