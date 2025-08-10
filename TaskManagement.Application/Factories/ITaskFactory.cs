

using TaskManagement.Application.Dtos;

namespace TaskManagement.Application.Factories
{
    public interface ITaskFactory
    {
        Task Create(TaskDto<int> dto, string? preset = null);
    }
}
