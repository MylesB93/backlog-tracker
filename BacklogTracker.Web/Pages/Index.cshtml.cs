using BacklogTracker.Application.Entities;
using BacklogTracker.Application.Interfaces;
using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Pages
{
	public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public GameCollectionDto? GamesResponse { get; set; }

        public IndexModel(IGameService gameService, ILogger<IndexModel> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        public async Task OnPost()
        {            
            string? query = Request.Form["query"];

            if (!string.IsNullOrEmpty(query))
            { 
                try
                {
					_logger.LogInformation("Fetching games from GiantBomb API...");
					GamesResponse = await _gameService.GetGamesAsync(Request.Form["query"]);
                    ViewData["NoResults"] = GamesResponse == null ? "" : "No Games Matching The Query!";
				}
                catch (Exception ex)
                {
					_logger.LogError(ex, $"Error occurred while fetching games - {ex.Message}");
				}
			}
		}
    }
}
