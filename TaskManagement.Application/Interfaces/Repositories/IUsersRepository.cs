using System.Linq.Expressions;
using TaskManagement.Application.Dtos;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<Users>
    {
        Task<Users?> GetByCredentialsAsync(string email, string password);
    }
}
