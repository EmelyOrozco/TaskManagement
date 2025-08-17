
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Extentions;
using TaskManagement.Application.Factories;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class TaskService : BaseService<TaskDto<int>, Tasks>, ITaskService
    {
        private readonly ITasksRepository _repository;
        private readonly ILogger<TaskService> _logger;
        private readonly Func<TaskDto<int>, bool> _Validate;
        private readonly Action<TaskDto<int>> _NotifyUpdate;
        private readonly ITaskFactory _factory;
        public TaskService(ITasksRepository repository, ILogger<TaskService> logger, Func<TaskDto<int>, bool> Validate, Action<TaskDto<int>> NotifyUpdate, ITaskFactory taskFactory) 
            : base(repository, logger)
        {
            _repository = repository;
            _logger = logger;
            _Validate = Validate;
            _NotifyUpdate = NotifyUpdate;
            _factory = taskFactory;
        }

        public async Task<OperationResult<TaskDto<int>>> CreateAsync(TaskDto<int> dto)
        {
            try
            {

                if (!_Validate(dto))
                {
                    return OperationResult<TaskDto<int>>.Failure("Datos inválidos para crear la tarea");
                }
                var queue = new Queue<TaskDto<int>>();
                queue.Enqueue(dto);

                TaskDto<int>? dtoOut = null;
                while (queue.Count > 0) 
                { 
                    var next = queue.Dequeue();

                    var entity = _factory.Create(next); //dto.ToEntityFromDTo();

                    var result = await _repository.AddAsync(entity);


                    if (!result.IsSuccess || result.Data is null)
                    {
                        _logger.LogError("No se pudo crear la tarea {Error}", result.Message);
                        return OperationResult<TaskDto<int>>.Failure("No se pudo crear la tarea");
                    }


                    var saved = (Tasks)result.Data;
                    dtoOut = saved.ToDtoFromEntity<int>();
                    await Task.Delay(1000); 
                }
                return OperationResult<TaskDto<int>>.Success("Tarea creada correctamente", dtoOut);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando la tarea");
                return OperationResult<TaskDto<int>>.Failure($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<TaskDto<int>>> UpdateAsync(int id, TaskDto<int> dto)
        {
            try
            {

                if (!_Validate(dto))
                {
                    _NotifyUpdate(dto);
                    return OperationResult<TaskDto<int>>.Failure("Datos inválidos para actualizar la tarea");
                }

                var entity = dto.ToEntityFromDTo();
                entity.TaskId = id;

                var result = await _repository.UpdateAsync(entity);

                if (!result.IsSuccess)
                {
                    return OperationResult<TaskDto<int>>.Failure("No se pudo actualizar la tarea");
                }
                else { 
                    return OperationResult<TaskDto<int>>.Success("Tarea actualizada correctamente", result.Data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error actualizando la tarea con ID {id}");
                return OperationResult<TaskDto<int>>.Failure($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<TaskDto<int>>> DeleteAsync(int id)
        {
            try
            {
                var getResult = await _repository.GetByIdAsync(id);

                if (!getResult.IsSuccess || getResult.Data == null)
                {
                    return OperationResult<TaskDto<int>>.Failure("Tarea no encontrada");
                }

                Action<TaskDto<int>> NotifyDelete = dto =>
                    _logger.LogError(getResult.Message);

                var deleteResult = await _repository.DeleteAsync(getResult.Data);

                if (!deleteResult.IsSuccess) 
                { 
                    return OperationResult<TaskDto<int>>.Failure("No se pudo eliminar la tarea");
                }
                return OperationResult<TaskDto<int>>.Success("Tarea eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando la tarea con ID {id}");
                return OperationResult<TaskDto<int>>.Failure($"Error: {ex.Message}");
            }
        }
  
    }
}
