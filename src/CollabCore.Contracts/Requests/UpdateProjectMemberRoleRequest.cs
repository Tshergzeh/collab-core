namespace CollabCore.Contracts.Requests
{
    public class UpdateProjectMemberRoleRequest
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}