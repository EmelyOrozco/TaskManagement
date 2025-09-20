using TaskManagement.Application.Dtos;
using TaskManagement.Application.Extentions;
using TaskManagement.Domain.Constants;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Factories
{
    public class TaskFactory : ITaskFactory
    {
            public Tasks Create(TaskDto<int> dto)
            {
                var e = dto.ToEntityFromDTo();

                e.Status = string.IsNullOrWhiteSpace(e.Status) ? "Open" : e.Status;

                if (e.Priority < Priorities.Low || e.Priority > Priorities.High)
                    e.Priority = Priorities.Medium;

                switch (e.Priority)
                {
                    case Priorities.High:
                        e.DueDate = DateTime.UtcNow.AddDays(1);
                        break;

                    case Priorities.Medium:
                        e.DueDate = DateTime.UtcNow.AddDays(7);
                        break;

                    case Priorities.Low:
                        e.DueDate = DateTime.UtcNow.AddDays(14);
                        break;

                }

                return e;
            }
        }
    }
