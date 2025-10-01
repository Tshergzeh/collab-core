using System.ComponentModel.DataAnnotations;

namespace CollabCore.Core.Entities
{
    public class Label
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Colour { get; set; } = "#000000";

        public ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
    }
}