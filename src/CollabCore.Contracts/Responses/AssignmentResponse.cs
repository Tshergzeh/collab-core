namespace CollabCore.Contracts.Responses
{
    public class AssignmentResponse
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
    }
}