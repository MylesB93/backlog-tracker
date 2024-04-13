using BacklogTracker.Models;

namespace BacklogTracker.Interfaces
{
	public interface IGameService
	{
		Task<Response> GetGamesAsync();
	}
}
