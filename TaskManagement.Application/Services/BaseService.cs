using Microsoft.Extensions.Logging;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Base;

namespace TaskManagement.Application.Services
{
    public abstract class BaseService<TDto, TEntity> : IBaseService<TDto> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly ILogger _logger;
        //protected readonly Func<TDto, IEnumerable<string>> _Validate;
        //protected readonly Action<int>? _OnDeleted;
        //protected readonly Action<TDto>? _OnUpdated;
        //protected readonly Action<TDto>? _OnCreated;

        protected BaseService(IBaseRepository<TEntity> repository,
            ILogger logger)
            //Func<TDto, IEnumerable<string>> validate,
            //Action<TDto>? onCreated,
            //Action<int>? onDeleted,
            //Action<TDto>? onUpdated)
        {
            _repository = repository;
            _logger = logger;
            //_Validate = validate;
            //_OnCreated = onCreated;
            //_OnDeleted = onDeleted;
            //_OnUpdated = onUpdated;
        }
        public async Task<OperationResult<List<TDto>>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync(x => true);
                if (result == null)
                {
                    return OperationResult<List<TDto>>.Failure("No se encontraron elementos");
                }
                return OperationResult<List<TDto>>.Success("Datos obtenidos", result.Data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return OperationResult<List<TDto>>.Failure($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<TDto>> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Getting entity of type {typeof(TEntity).Name} with ID {id}");
            try
            {
                if (id <= 0)
                {
                    return OperationResult<TDto>.Failure("The ID must be greater than 0");
                }
                var entity = await _repository.GetByIdAsync(id);
                if (entity is null)
                {
                    return OperationResult<TDto>.Failure($"No entity found with ID {id}");
                }
                return OperationResult<TDto>.Success("Entity retrieved successfully", entity.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving the entity with ID {id}: {ex.Message}");
                return OperationResult<TDto>.Failure($"Error retrieving entity: {ex.Message}");
            }
            
        }

        public async Task<OperationResult<TDto>> CreateAsync(TDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<TDto>> UpdateAsync(int id, TDto dto)
        {
           throw new NotImplementedException();
        }

        public async Task<OperationResult<TDto>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
    
}
