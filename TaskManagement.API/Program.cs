
using Microsoft.EntityFrameworkCore;
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


            builder.Services.AddTransient<ITaskService, TaskService>();

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
