

namespace TaskManagement.Application.Factories
{
    public class TaskFactory: ITaskFactory
    {
        public Task Create(TaskDto<int> dto, string? preset = null)
        {
            var e = dto.ToEntityFromDto(); 

            e.CreatedAt = e.CreatedAt == default ? DateTime.UtcNow : e.CreatedAt;
            e.Status = string.IsNullOrEmpty(e.Status) ? "Open" : e.Status;


            switch(preset?.ToLowerInvariant())
            {
                case "Alta":
                    e.Priority = "High";
                    break;
                case "Baja":
                    e.Priority = "Low";
                    break;
                case "Bug":
                    e.Priority = "Hight";
                    e.Status = "ToDo";
                    e.Description = ;
                    break;
                default:
                    e.Priority = "Normal";
                    break;
            }
            return new Task
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Preset = preset
            };
        }
    }
}
