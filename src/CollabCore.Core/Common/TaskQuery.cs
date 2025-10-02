namespace CollabCore.Core.Common
{
    public class TaskQuery
    {
        public Guid ProjectId { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}