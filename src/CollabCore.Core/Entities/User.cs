using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CollabCore.Core.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public required byte[] PasswordHash { get; set; }

        [Required, MaxLength(255)]
        public required byte[] PasswordSalt { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; } = "Contributor"; // Contributor, PM, Admin

        [Required, MaxLength(255)]
        public required string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiryTime { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
    }
}
