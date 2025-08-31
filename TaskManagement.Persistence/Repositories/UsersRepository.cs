using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Dtos;
using TaskManagement.Application.Extentions;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.Entities;
using TaskManagement.Persistence.Base;
using TaskManagement.Persistence.Context;

namespace TaskManagement.Persistence.Repositories
{
    public class UsersRepository : BaseRepository<Users>, IUsersRepository
    {
        public UsersRepository(TaskContext context) : base(context)
        {

        }

        public async Task<Users?> GetByCredentialsAsync(string email, string password)
        {
           return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

    }

}
