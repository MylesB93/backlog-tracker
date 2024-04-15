using BacklogTracker.Models;
using BacklogTracker.Models.UserBacklog;

namespace BacklogTracker.Interfaces
{
	public interface IGameService
	{
		Task<Response> GetGamesAsync(string? query);
		Task<List<BacklogGame>> GetUsersGamesAsync(List<string> gameIds);
	}
}
