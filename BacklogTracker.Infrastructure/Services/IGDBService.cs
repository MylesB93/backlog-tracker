using BacklogTracker.Application.Data.DTOs;
using BacklogTracker.Application.Interfaces;
using BacklogTracker.Infrastructure.Configuration;
using BacklogTracker.Models;
using Microsoft.Extensions.Caching.Memory;
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
		private readonly IMemoryCache _cache;

		public IGDBService(ILogger<IGDBService> logger, IOptions<IGDBConfiguration> igdbConfiguration, IHttpClientFactory httpClientFactory, IMemoryCache cache) 
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
			_igdbConfiguration = igdbConfiguration;
			_cache = cache;
		}

		public async Task<GameCollectionDto> GetGamesAsync(string? query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return new GameCollectionDto();
			}

			var cacheKey = $"igdb_games_{query.ToLowerInvariant()}";

			if (_cache.TryGetValue(cacheKey, out GameCollectionDto? cachedResult) && cachedResult != null)
			{
				_logger.LogInformation($"Returning cached results for query: {query}");
				return cachedResult;
			}

			var client = _httpClientFactory.CreateClient("IGDB");

			var encodedQuery = Uri.EscapeDataString(query);
			var response = await client.GetAsync($"games?fields=name,url,storyline;&search={encodedQuery};&limit={_igdbConfiguration.Value.GameLimit};");

            var body = await response.Content.ReadAsStringAsync();

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

				var result = new GameCollectionDto { Games = games };

				var cacheOptions = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
				};

				_cache.Set(cacheKey, result, cacheOptions);

				_logger.LogInformation("Request complete!");

				return result;
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

			if (gameIds == null || gameIds.Count == 0)
			{
				return new GameCollectionDto();
			}

			var numericIds = new List<string>();
			foreach (var id in gameIds)
			{
				if (!string.IsNullOrWhiteSpace(id) && long.TryParse(id, out _))
				{
					numericIds.Add(id.Trim());
				}
			}

			if (numericIds.Count == 0)
			{
				return new GameCollectionDto();
			}

			var cacheKey = $"igdb_users_games_{string.Join("_", numericIds)}";

			if (_cache.TryGetValue(cacheKey, out GameCollectionDto? cachedResult) && cachedResult != null)
			{
				return cachedResult;
			}

			var idsQuery = $"where id = ({string.Join(",", numericIds)}); fields name, url, storyline;";
			var content = new StringContent(idsQuery);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

			var response = await client.PostAsync("games", content);

			if (!response.IsSuccessStatusCode)
			{
				return new GameCollectionDto();
			}

			try
			{
				var jsonString = await response.Content.ReadAsStringAsync();
				var igdbGames = JsonSerializer.Deserialize<List<IGDBGame>>(jsonString);

				var games = igdbGames?.Select(g => new GameDto
				{
					Id = g.Id.ToString(),
					Name = g.Name,
					Url = g.Url,
					Description = g.Storyline
				}).ToList();

				var result = new GameCollectionDto { Games = games };

				var cacheOptions = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
				};

				_cache.Set(cacheKey, result, cacheOptions);

				return result;
			}
			catch
			{
				return new GameCollectionDto();
			}
		}
	}
}
