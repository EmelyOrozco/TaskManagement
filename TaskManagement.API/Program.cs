using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Factories;
using TaskManagement.Application.Hubs;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Services;
using TaskManagement.Persistence.Context;
using TaskManagement.Persistence.Repositories;


namespace TaskManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TaskContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbTask")));
            // Add services to the container.
            builder.Services.AddScoped<ITasksRepository, TaskRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddSignalR();

            builder.Services.AddScoped<ITaskFactory, Application.Factories.TaskFactory>();
            builder.Services.AddTransient<ITaskService, TaskService>();
            builder.Services.AddScoped<IUsersService, UserService>();

            builder.Services.AddScoped<Func<TaskDto<int>, bool>>(sp => dto =>
            { 
                return dto is not null
                    && !string.IsNullOrWhiteSpace(dto.Description)
                    && dto.Status is not null
                    && dto.UserId > 0;
            });

            builder.Services.AddScoped<Action<TaskDto<int>>>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<TaskService>>();

                return dto =>
                {
                    logger.LogInformation(
                        "Notificación - UserId:{UserId} Status:{Status} Due:{DueDate:O}",
                        dto?.UserId, dto?.Status, dto?.DueDate);
                };
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapHub<TaskHub>("/taskhub");

            app.UseAuthentication();
            app.UseAuthorization();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
