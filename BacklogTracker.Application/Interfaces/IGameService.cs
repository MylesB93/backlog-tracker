using BacklogTracker.Application.Data.DTOs;

namespace BacklogTracker.Application.Interfaces
{
	public interface IGameService
	{
		Task<GameCollectionDto> GetGamesAsync(string? query);
		Task<GameCollectionDto> GetUsersGamesAsync(List<string> gameIds);
	}
}
