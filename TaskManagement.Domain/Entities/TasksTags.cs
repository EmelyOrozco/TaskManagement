

using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class TasksTags
    {
        [Key]
        public int TaskId { get; set; }

        public int TagId { get; set; }

        public Guid Rowguid { get; set; }
    }
}
