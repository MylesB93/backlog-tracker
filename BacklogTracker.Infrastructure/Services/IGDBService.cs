using BacklogTracker.Application.Interfaces;
using BacklogTracker.Infrastructure.Configuration;
using BacklogTracker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BacklogTracker.Infrastructure.Services
{
	public class IGDBService : IGameService
	{
		private readonly ILogger<IGDBService> _logger;
		private IOptions<IGDBConfiguration> _igdbConfiguration;
		private readonly IHttpClientFactory _httpClientFactory;

		public IGDBService(ILogger<IGDBService> logger, IOptions<IGDBConfiguration> igdbConfiguration, IHttpClientFactory httpClientFactory) 
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
			_igdbConfiguration = igdbConfiguration;
		}

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
