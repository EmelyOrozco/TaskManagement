using Microsoft.Extensions.Logging;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Extentions;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Services
{
    public class UserService : BaseService<UsersDto<int>, Users>, IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUsersRepository usersRepository, ILogger<UserService> logger) : base(usersRepository, logger)
        {
            _repository = usersRepository;
            _logger = logger;
        }

        public async Task<OperationResult<UsersDto<int>>> CreateAsync(UsersDto<int> dto)
        {
            try
            {
                var exist = await _repository.ExistsAsync(x => x.Username == dto.Username);

                if (exist.Data)
                {
                    return OperationResult<UsersDto<int>>.Failure($"Ya existe un Usuario con este user name, intente con otro");
                }

                var entity = dto.ToUsersEntityFromDTo();

                var result = await _repository.AddAsync(entity);


                if (!result.IsSuccess || result.Data is null)
                {
                   _logger.LogError("No se pudo crear el usuario {Error}", result.Message);
                   return OperationResult<UsersDto<int>>.Failure("No se pudo crear el usuario");
                }

                var saved = (Users)result.Data;
                return OperationResult<UsersDto<int>>.Success("Usuario creado correctamente", saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando el usuario");
                return OperationResult<UsersDto<int>>.Failure($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<UsersDto<int>>> UpdateAsync(int id, UsersDto<int> dto)
        {
            try
            {

                var entity = dto.ToUsersEntityFromDTo();
                entity.UserId = id;

                var result = await _repository.UpdateAsync(entity);

                if (!result.IsSuccess)
                {
                    return OperationResult<UsersDto<int>>.Failure("No se pudo actualizar el usuario");
                }
                else
                {
                    return OperationResult<UsersDto<int>>.Success("Usuario actualizado correctamente", result.Data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error actualizando el usuario con ID {id}");
                return OperationResult<UsersDto<int>>.Failure($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<UsersDto<int>>> DeleteAsync(int id)
        {
            try
            {
                var getResult = await _repository.GetByIdAsync(id);
                _logger.LogInformation($"Obteniendo el usuario con ID {id}");
                if (!getResult.IsSuccess)
                {
                    return OperationResult<UsersDto<int>>.Failure("Error al obtener el usuario");
                }

                if (!getResult.IsSuccess || getResult.Data == null)
                {
                    return OperationResult<UsersDto<int>>.Failure("Usuario no encontrado");
                }

                Action<UsersDto<int>> NotifyDelete = dto =>
                    _logger.LogError(getResult.Message);

                var deleteResult = await _repository.DeleteAsync(getResult.Data);

                if (!deleteResult.IsSuccess)
                {
                    return OperationResult<UsersDto<int>>.Failure("No se pudo eliminar el usuario");
                }
                return OperationResult<UsersDto<int>>.Success("Usuario eliminado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando el usuario con ID {id}");
                return OperationResult<UsersDto<int>>.Failure($"Error: {ex.Message}");
            }
        }
        public async Task<OperationResult<UsersDto<int>>> ValidateUserAsync(string email, string password)
        {
            try
            {
                var usersResult = await _repository.GetByCredentialsAsync(email, password);

                if (usersResult == null)
                    return OperationResult<UsersDto<int>>.Failure("No se pudieron obtener los usuarios");

                var userDto = usersResult.ToUsersDtoFromEntity<int>();

                return OperationResult<UsersDto<int>>.Success("Usuario válido", userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando usuario");
                return OperationResult<UsersDto<int>>.Failure($"Error: {ex.Message}");
            }
        }

    }
}
