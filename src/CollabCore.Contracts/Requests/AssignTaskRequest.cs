namespace CollabCore.Contracts.Requests
{
    public class AssignTaskRequest
    {
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}