
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Persistence.Base;
using TaskManagement.Persistence.Context;

namespace TaskManagement.Persistence.Repositories
{
    public class TaskRepository: BaseRepository<Tasks>, ITasksRepository
    {
        public TaskRepository(TaskContext context): base(context)
        {
            
        }
    }
}
