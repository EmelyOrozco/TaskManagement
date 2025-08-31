
using TaskManagement.Application.Dtos;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Extentions
{
    public static class UserExtention
    {
        public static Users ToUsersEntityFromDTo<T>(this UsersDto<T> usersDto)
        {

            return new Users
            {
                UserId = usersDto.UserId,
                Username = usersDto.Username,
                Email = usersDto.Email,
                Password = usersDto.Password,
                Rowguid = usersDto.Rowguid
            };
        }

        public static UsersDto<T> ToUsersDtoFromEntity<T>(this Users users)
        {
            return new UsersDto<T>
            {
                UserId = users.UserId,
                Username = users.Username,
                Email = users.Email,
                Password = users.Password,
                Rowguid = users.Rowguid

            };
        }
    }
}
