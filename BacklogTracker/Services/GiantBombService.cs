using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;

namespace BacklogTracker.Services
{
	public class GiantBombService : IGameService
	{
		private readonly ILogger<GiantBombService> _logger;
		private IOptions<GiantBombConfiguration> _giantBombConfiguration;

		public GiantBombService(ILogger<GiantBombService> logger, IOptions<GiantBombConfiguration> giantBombConfiguration) 
		{ 
			_logger = logger;
			_giantBombConfiguration = giantBombConfiguration;
		}

		public async Task<Response> GetGamesAsync(string? query)
		{
			var gamesResponse = new Response();

			using var client = new HttpClient();
			client.BaseAddress = new Uri("https://www.giantbomb.com/api/search/");
			client.DefaultRequestHeaders.Add("User-Agent", "Backlog Tracker app");
			// Get data response
			var response = client.GetAsync($"?api_key={_giantBombConfiguration.Value.GiantBombAPIKey}&query={query}&resources=game&field_list=name,site_detail_url,description,guid").Result;

			XmlSerializer xs = new XmlSerializer(typeof(Response));

			using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
			{
				try
				{
					gamesResponse = (Response)xs.Deserialize(reader);
					_logger.LogInformation("Deserialization complete!");
				}
				catch (Exception ex)
				{
					_logger.LogError("Error occurred during deserialization: {0}", ex.Message);
				}				
			}

			_logger.LogInformation("Request complete!");

			return gamesResponse;
		}
	}
}
