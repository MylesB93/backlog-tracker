namespace BacklogTracker.Infrastructure.Configuration
{
	public class IGDBConfiguration
	{
		public string? ClientID { get; set; }
		public string? Authorization { get; set; }
		public int GameLimit { get; set; } = 100;
	}
}
