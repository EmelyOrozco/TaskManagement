using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Factories;
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

            builder.Services.AddScoped<ITaskFactory, Application.Factories.TaskFactory>();
            builder.Services.AddTransient<ITaskService, TaskService>();

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
