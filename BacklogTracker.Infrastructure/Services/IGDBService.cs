using BacklogTracker.Application.Interfaces;
using BacklogTracker.Models;

namespace BacklogTracker.Infrastructure.Services
{
	public class IGDBService : IGameService
	{
		public Task<Response> GetGamesAsync(string? query)
		{
			throw new NotImplementedException();
		}

		public Task<Response> GetUsersGamesAsync(List<string> gameIds)
		{
			throw new NotImplementedException();
		}
	}
}
