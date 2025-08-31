

namespace TaskManagement.Application.Dtos
{
    public record class TaskDto<T>
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }

        public int? Priority { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid Rowguid { get; set; } = Guid.NewGuid();

    }
}
