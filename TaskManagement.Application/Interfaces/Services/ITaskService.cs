

using TaskManagement.Application.Dtos;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService: IBaseService<TaskDto<int>>
    {
    }
}
