namespace BacklogTracker.Application.Data.DTOs
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? GameID { get; set; }
        public List<string> GameIDs { get; set; } = new();
        public List<string> CompletedGameIDs { get; set; } = new();

	}
}
