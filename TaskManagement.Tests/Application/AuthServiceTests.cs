using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Base;

namespace TaskManagement.Tests.Application
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task GenerateTokenAsync_ValidCredentials_ReturnsSignedTokenWithClaims()
        {
            var mockUserService = new Mock<IUsersService>();
            var mockConfig = new Mock<IConfiguration>();

            var email = "test@example.com";
            var password = "password123";
            var userDto = new UsersDto<int>
            {
                UserId = 1,
                Email = email,
                Username = "testuser",
                Password = password
            };

            mockUserService
                .Setup(s => s.ValidateUserAsync(email, password))
                .ReturnsAsync(OperationResult<UsersDto<int>>.Success("ok", userDto));

            const string issuer = "TestIssuer";
            const string key = "a3f9b1c4d7e2f8659b0a12d48c3f7e9b5d6a1c2f3b4e5d6a7c8b9e0f1a2b3c4d"; 
            mockConfig.Setup(c => c["Jwt:Key"]).Returns(key);
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns(issuer);

            var sut = new AuthService(mockUserService.Object, mockConfig.Object);

      
            var tokenString = await sut.GenerateTokenAsync(email, password);

      
            Assert.False(string.IsNullOrWhiteSpace(tokenString));
            mockUserService.Verify(s => s.ValidateUserAsync(email, password), Times.Once);

          
            var handler = new JwtSecurityTokenHandler();
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false, 
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = handler.ValidateToken(tokenString, validationParams, out var validatedToken);
            Assert.NotNull(validatedToken);

          
            Assert.Equal("1", principal.FindFirst("UserId")?.Value);
            Assert.Equal(email, principal.FindFirst("Email")?.Value);
            Assert.Equal("testuser", principal.FindFirst("Username")?.Value);


            var jwt = (JwtSecurityToken)validatedToken;
            Assert.True(jwt.ValidTo > DateTime.UtcNow.AddMinutes(100));
        }
    }
}
