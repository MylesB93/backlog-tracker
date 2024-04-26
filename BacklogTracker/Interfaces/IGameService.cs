using BacklogTracker.Models;

namespace BacklogTracker.Interfaces
{
	public interface IGameService
	{
		Task<Response> GetGamesAsync(string? query);
		Task<Response> GetUsersGamesAsync(List<string> gameIds);
	}
}
