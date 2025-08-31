using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Guid Rowguid { get; set; }

    }
}
