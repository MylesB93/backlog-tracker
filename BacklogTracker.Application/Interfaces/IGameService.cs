using BacklogTracker.Application.Entities;

namespace BacklogTracker.Application.Interfaces
{
	public interface IGameService
	{
		Task<GameCollectionDto> GetGamesAsync(string? query);
		Task<GameCollectionDto> GetUsersGamesAsync(List<string> gameIds);
	}
}
