using System.Linq.Expressions;
using TaskManagement.Domain.Base;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IBaseService<TDto>
    {
        Task<OperationResult<TDto>> GetByIdAsync(int id);
        Task<OperationResult<List<TDto>>> GetAllAsync();
        Task<OperationResult<TDto>> CreateAsync(TDto dto);
        Task<OperationResult<TDto>> UpdateAsync(int id, TDto dto);
        Task<OperationResult<TDto>> DeleteAsync(int id);
        //Task<bool> ExistsAsync(int id);
    }
}
