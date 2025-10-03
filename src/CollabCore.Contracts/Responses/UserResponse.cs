namespace CollabCore.Contracts.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "Contributor";
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}