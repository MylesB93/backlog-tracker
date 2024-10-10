using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace BacklogTracker.Services
{
	public class GiantBombService : IGameService
    {
        private readonly ILogger<GiantBombService> _logger;
        private IOptions<GiantBombConfiguration> _giantBombConfiguration;
		private readonly IHttpClientFactory _httpClientFactory;

		public GiantBombService(ILogger<GiantBombService> logger, IOptions<GiantBombConfiguration> giantBombConfiguration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _giantBombConfiguration = giantBombConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Response> GetGamesAsync(string? query)
        {
            var games = new Response();

            var client = _httpClientFactory.CreateClient("GiantBomb");

            var response = client.GetAsync($"/api/search/?api_key={_giantBombConfiguration.Value.GiantBombAPIKey}&query={query}&resources=game&field_list=name,site_detail_url,description,id").Result;

            XmlSerializer xs = new XmlSerializer(typeof(Response));

            using (StreamReader reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                try
                {
                    games = (Response?)xs.Deserialize(reader);
                    _logger.LogInformation("Deserialization complete!");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred during deserialization: {ex.Message}");
                }
            }

            _logger.LogInformation("Request complete!");

            return games ?? new Response();
        }

        public async Task<Response> GetUsersGamesAsync(List<string> gameIds)
        {
            var gamesList = new Response();

            var client = _httpClientFactory.CreateClient("GiantBomb");

            var response = await client.GetAsync($"/api/games/?api_key={_giantBombConfiguration.Value.GiantBombAPIKey}&filter=id:{string.Join("|",gameIds)}&field_list=name,site_detail_url,description,guid,id");

            XmlSerializer xs = new XmlSerializer(typeof(Response));

            using (StreamReader reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                try
                {
                    gamesList = (Response?)xs.Deserialize(reader);
                    _logger.LogInformation("Deserialization complete!");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred during deserialization: {ex.Message}");
                }
            }

            _logger.LogInformation("Request complete!");


            return gamesList ?? new Response();
        }
    }
}
