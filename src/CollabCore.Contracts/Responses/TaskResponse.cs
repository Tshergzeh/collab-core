namespace CollabCore.Contracts.Responses
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Todo";
        public string Priority { get; set; } = "Medium";
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CreatedBy { get; set; }
    }
}