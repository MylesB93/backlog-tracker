using BacklogTracker.Application.Interfaces;
using BacklogTracker.Infrastructure.Configuration;
using BacklogTracker.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

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

		public async Task<Response> GetGamesAsync(string? query)
		{
			var client = _httpClientFactory.CreateClient("IGDB");

			//TODO: Move headers to secrets
			client.DefaultRequestHeaders.Add("Client-ID", "xmq9cokqifli4gcm219wsjxbj9xp63");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer 36nyegwmx2q11ves4x9ap4p05uzrp2");

			var response = await client.GetAsync($"games?fields=name;&search={query};&limit=5;");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError($"IGDB API request failed with status code {response.StatusCode}");
				return new Response();
			}

			try
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				var igdbGames = JsonSerializer.Deserialize<List<IGDBGame>>(jsonString);

				_logger.LogInformation("Deserialization complete!");

				// Map IGDB games to your existing Response/Game structure
				var games = igdbGames?.Select(g => new Game
				{
					Id = g.Id.ToString(),
					Name = g.Name
				}).ToList();

				_logger.LogInformation("Request complete!");

				return new Response { Games = games };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error occurred during deserialization: {ex.Message}");
				return new Response();
			}
		}

		public async Task<Response> GetUsersGamesAsync(List<string> gameIds)
		{
			throw new NotImplementedException();

			//var gamesList = new Response();

			//var client = _httpClientFactory.CreateClient("GiantBomb");

			//var response = await client.GetAsync($"/api/games/?api_key={_igdbConfiguration.Value.GiantBombAPIKey}&filter=id:{string.Join("|", gameIds)}&field_list=name,site_detail_url,description,guid,id");

			//XmlSerializer xs = new XmlSerializer(typeof(Response));

			//using (StreamReader reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
			//{
			//	try
			//	{
			//		gamesList = (Response?)xs.Deserialize(reader);
			//		_logger.LogInformation("Deserialization complete!");
			//	}
			//	catch (Exception ex)
			//	{
			//		_logger.LogError($"Error occurred during deserialization: {ex.Message}");
			//	}
			//}

			//_logger.LogInformation("Request complete!");


			//return gamesList ?? new Response();
		}
	}
}
