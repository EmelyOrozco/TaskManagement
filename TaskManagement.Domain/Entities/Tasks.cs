

namespace TaskManagement.Domain.Entities
{
    public class Tasks
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }

        public int? Priority { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid Rowguid { get; set; }
    }
}
