using TaskManagement.Application.Dtos;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IUsersService: IBaseService<UsersDto<int>>
    {
        Task<OperationResult<UsersDto<int>>> ValidateUserAsync(string email, string password);
    }
}
