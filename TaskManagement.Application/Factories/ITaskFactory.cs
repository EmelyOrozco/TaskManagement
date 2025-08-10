

using TaskManagement.Application.Dtos;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Factories
{
    public interface ITaskFactory
    {
        Tasks Create(TaskDto<int> dto, string? preset = null);
    }
}
