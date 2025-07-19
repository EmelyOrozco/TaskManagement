using TaskManagement.Application.Dtos;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Extentions
{
    public static class TaskExtention
    {
        public static Tasks ToEntityFromDTo<T>(this TaskDto<T> taskDto)
        {
            
            return new Tasks
            {
                TaskId = taskDto.TaskId,
                UserId = taskDto.UserId,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                CreatedAt = taskDto.CreatedAt,
                Rowguid = taskDto.Rowguid
            };
        }

        public static TaskDto<T> ToDtoFromEntity<T>(this Tasks task)
        {
            return new TaskDto<T>
            {
                TaskId = task.TaskId,
                UserId = task.UserId,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                Rowguid = task.Rowguid
            };
        }
    }
}
