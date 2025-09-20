using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUsersService _userservice;
        private readonly IConfiguration _configuration;
        public AuthService(IUsersService userService, IConfiguration configuration)
        {
            _userservice = userService;
            _configuration = configuration;
        }
        
        public async Task<string> GenerateTokenAsync(string email, string password)
        {
            var result = await _userservice.ValidateUserAsync(email, password);

            if (!result.IsSuccess || result.Data == null)
                return null;

            var user = result.Data;

            var secretkey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Email", user.Email),
                new Claim("Username", user.Username)

            };

            var token = new JwtSecurityToken(
            issuer: issuer,
            claims: claims,
            audience: _configuration["Jwt:Audience"],
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(120),
            signingCredentials: credentials
            
            );
                                                                                         
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
