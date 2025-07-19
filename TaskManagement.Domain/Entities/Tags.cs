

using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class Tags
    {
        [Key]
        public int TagId { get; set; }

        public string Name { get; set; }

        public Guid Rowguid { get; set; }
    }
}
