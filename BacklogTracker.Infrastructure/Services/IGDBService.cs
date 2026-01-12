using BacklogTracker.Application.Data.DTOs;
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

		public async Task<GameCollectionDto> GetGamesAsync(string? query)
		{
			var client = _httpClientFactory.CreateClient("IGDB");

			client.DefaultRequestHeaders.Remove("Client-ID");
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Client-ID", _igdbConfiguration.Value.ClientID);
			client.DefaultRequestHeaders.Add("Authorization", _igdbConfiguration.Value.Authorization);

			var encodedQuery = Uri.EscapeDataString(query ?? string.Empty);
			var response = await client.GetAsync($"games?fields=name,url,storyline;&search={encodedQuery};&limit={_igdbConfiguration.Value.GameLimit};");

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError($"IGDB API request failed with status code {response.StatusCode}");
				return new GameCollectionDto();
			}

			try
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				var igdbGames = JsonSerializer.Deserialize<List<IGDBGame>>(jsonString);

				_logger.LogInformation("Deserialization complete!");

				// Map IGDB games to your existing Response/Game structure
				var games = igdbGames?.Select(g => new GameDto
				{
					Id = g.Id.ToString(),
					Name = g.Name,
					Url = g.Url,
					Description = g.Storyline
                }).ToList();

				_logger.LogInformation("Request complete!");

				return new GameCollectionDto { Games = games };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error occurred during deserialization: {ex.Message}");
				return new GameCollectionDto();
			}
		}

		public async Task<GameCollectionDto> GetUsersGamesAsync(List<string> gameIds)
		{
			var client = _httpClientFactory.CreateClient("IGDB");

            client.DefaultRequestHeaders.Remove("Client-ID");
            client.DefaultRequestHeaders.Remove("Authorization");
			client.DefaultRequestHeaders.Add("Client-ID", _igdbConfiguration.Value.ClientID);
			client.DefaultRequestHeaders.Add("Authorization", _igdbConfiguration.Value.Authorization);

            var idsQuery = $"where id = ({string.Join(",", gameIds)}); fields name, url, storyline;";
			var content = new StringContent(idsQuery);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

			var response = await client.PostAsync("games", content);

			if (!response.IsSuccessStatusCode)
			{
				_logger.LogError($"IGDB API request failed with status code {response.StatusCode}");
				return new GameCollectionDto();
			}

			try
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				var igdbGames = JsonSerializer.Deserialize<List<IGDBGame>>(jsonString);

				_logger.LogInformation("Deserialization complete!");

				var games = igdbGames?.Select(g => new GameDto
				{
					Id = g.Id.ToString(),
					Name = g.Name,
					Url = g.Url,
					Description = g.Storyline
				}).ToList();

				_logger.LogInformation("Request complete!");

				return new GameCollectionDto { Games = games };
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error occurred during deserialization: {ex.Message}");
				return new GameCollectionDto();
			}
		}
	}
}
