using System.ComponentModel.DataAnnotations;

namespace CollabCore.Core.Entities
{
    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Todo"; // Todo, InProgress, Done

        [Required, MaxLength(50)]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High

        public DateTime? DueDate { get; set; }

        public Guid CreatedBy { get; set; }
        public User Creator { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
    }
}