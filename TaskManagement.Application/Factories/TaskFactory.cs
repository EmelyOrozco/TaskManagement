using TaskManagement.Application.Dtos;
using TaskManagement.Application.Extentions;
using TaskManagement.Domain.Constants;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Factories
{
    public class TaskFactory: ITaskFactory
    {
       
         public Tasks Create(TaskDto<int> dto, string? preset = null)
        {
            var e = dto.ToEntityFromDTo();

            e.CreatedAt = e.CreatedAt == default ? DateTime.UtcNow : e.CreatedAt;
            e.Status = string.IsNullOrWhiteSpace(e.Status) ? "Open" : e.Status;

            if (e.Priority < Priorities.Low || e.Priority > Priorities.High)
                e.Priority = Priorities.Medium;

            switch (preset?.ToLowerInvariant())
            {
                case "Alta":
                    e.Priority = Priorities.High;
                    e.DueDate = DateTime.UtcNow.AddDays(1);
                    break;

                case "Media":
                    e.Priority = Priorities.Medium;
                    e.DueDate = DateTime.UtcNow.AddDays(7);
                    break;

                case "Baja":
                    e.Priority = Priorities.Low;
                    e.DueDate = DateTime.UtcNow.AddDays(14);
                    break;

                case "Bug":
                    e.Priority = Priorities.High;
                    e.Status = "ToDo";
                    e.Description = string.IsNullOrWhiteSpace(e.Description)? "[BUG]" : $"[BUG] {e.Description}";
                    e.DueDate = DateTime.UtcNow.AddDays(2);
                    break;
            }

            return e;
        }
    }
}
