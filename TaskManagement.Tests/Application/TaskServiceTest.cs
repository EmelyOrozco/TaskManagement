using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using TaskManagement.Application;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Factories;
using TaskManagement.Application.Hubs;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.Entities;
using Xunit;

namespace TaskManagement.Tests.Application
{
    public class TaskServiceTest
    {
        [Fact]
        public async Task CreateAsync_ShouldReturnSuccess_WhenTaskIsValidAndNotExists()
        {
            var dto = new TaskDto<int> { Description = "Nueva tarea", Status = "Pendiente", UserId = 1 };
            var repoMock = new Mock<ITasksRepository>();
            repoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Tasks, bool>>>()))
                .ReturnsAsync(OperationResult<bool>.Success("Consulta de existencia", false));
            repoMock.Setup(r => r.AddAsync(It.IsAny<Tasks>()))
                .ReturnsAsync(OperationResult<Tasks>.Success("Tarea creada correctamente", new Tasks { Description = dto.Description, Status = dto.Status, UserId = dto.UserId }));
            var service = GetTaskService(repoMock.Object);

            var result = await service.CreateAsync(dto);

            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Tarea creada correctamente", result.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnFailure_WhenTaskIsInvalid()
        {
            var dto = new TaskDto<int> { Description = "", Status = null, UserId = 0 };
            var repoMock = new Mock<ITasksRepository>();
            var service = GetTaskService(repoMock.Object);

            var result = await service.CreateAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Datos inválidos para crear la tarea", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenUpdateIsValid()
        {
            var dto = new TaskDto<int> { Description = "Actualizar", Status = "Hecho", UserId = 2 };
            var repoMock = new Mock<ITasksRepository>();
            repoMock.Setup(r => r.UpdateAsync(It.IsAny<Tasks>()))
                .ReturnsAsync(OperationResult<Tasks>.Success("Tarea actualizada correctamente", new Tasks { Description = dto.Description, Status = dto.Status, UserId = dto.UserId }));
            var service = GetTaskService(repoMock.Object);

            var result = await service.UpdateAsync(1, dto);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Tarea actualizada correctamente", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFailure_WhenTaskNotFound()
        {
            var repoMock = new Mock<ITasksRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<Tasks>.Failure("No encontrada"));
            var service = GetTaskService(repoMock.Object);

            var result = await service.DeleteAsync(99);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Error al obtener la tarea", result.Message);
        }

        private TaskService GetTaskService(ITasksRepository repo)
        {
            var logger = new Mock<ILogger<TaskService>>().Object;
            Func<TaskDto<int>, bool> validate = dto => dto != null && !string.IsNullOrWhiteSpace(dto.Description) && dto.Status != null && dto.UserId > 0;
            Action<TaskDto<int>> notify = dto => { };   
            var factory = new Mock<ITaskFactory>().Object;
            var hub = new Mock<IHubContext<TaskHub>>().Object;
            return new TaskService(repo, logger, validate, notify, factory, hub);
        }
    }
}
