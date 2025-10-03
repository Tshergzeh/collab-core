namespace CollabCore.Contracts.Responses
{
    public class ProjectMembershipResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
    }
}