namespace CollabCore.Core.Entities
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string Role { get; set; } = "Contributor"; // Contributor or PM
    }
}