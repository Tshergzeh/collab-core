namespace CollabCore.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string Role { get; set; } = "Contributor";
    }
}
