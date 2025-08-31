using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Base;
using TaskManagement.Persistence.Context;

namespace TaskManagement.Persistence.Base
{
    public class BaseRepository<TEntity>: IBaseRepository<TEntity> where TEntity : class
    {
        public readonly TaskContext _context;
        public readonly DbSet<TEntity> _dbSet;
        public BaseRepository(TaskContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();

        }
        public virtual async Task<OperationResult<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {

                var data = await _dbSet.Where(filter).ToListAsync();
                return OperationResult<List<TEntity>>.Success($"Entities {typeof(TEntity)} retrieved successfully", data);
            }
            catch (Exception ex)
            {
                
                return OperationResult<List<TEntity>>.Failure($"Error retrieving entity: {typeof(TEntity)} - {ex.Message}");
            }
        }
        public virtual async Task<OperationResult<TEntity>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is null)
                {
                    return OperationResult<TEntity>.Failure("The entity not found in the database");
                }
                return OperationResult<TEntity>.Success($"Entity {typeof(TEntity)} retrieved successfully", entity);
            }
            catch (Exception)
            {
                return OperationResult<TEntity>.Failure($"Error retrieving entity: {typeof(TEntity)}");

            }
        }
        public virtual async Task<OperationResult<TEntity>> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<TEntity>.Success($"Entity {typeof(TEntity)} added successfully", entity);
            }
            catch (Exception ex)
            {

                var innerMessage = ex.InnerException?.Message ?? ex.Message;

               // _logger.LogError(ex, $"Error al guardar entidad {typeof(TEntity)}: {innerMessage}");

                return OperationResult<TEntity>.Failure($"Error agregando entidad: {innerMessage}");
            }
        }
        public virtual async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return OperationResult<TEntity>.Success($"Entity {typeof(TEntity)} updated successfully", entity);
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return OperationResult<TEntity>.Failure($"Error updating entity: {innerMessage}");
            }
        }
        public virtual async Task<OperationResult<TEntity>> DeleteAsync(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<TEntity>.Success($"Entity {typeof(TEntity)} delete successfully", entity);
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.Failure($"Error updating entity: {typeof(TEntity)} - {ex.Message}");
            }
        }
        //public virtual async Task<bool> ExistsAsync(int id)
        //{

           

        //}

    }
}
