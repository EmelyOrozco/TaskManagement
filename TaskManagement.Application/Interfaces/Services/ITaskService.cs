

using TaskManagement.Application.Dtos;
using TaskManagement.Domain.Base;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService: IBaseService<TaskDto<int>>
    {
    }
}
