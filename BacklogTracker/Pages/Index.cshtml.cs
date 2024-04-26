using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;

namespace BacklogTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public Response GamesResponse { get; set; }

        public IndexModel(IGameService gameService, ILogger<IndexModel> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async void OnPost()
        {
            string? query = Request.Form["query"];

            if (!string.IsNullOrEmpty(query))
            { 
                try
                {
					GamesResponse = await _gameService.GetGamesAsync(Request.Form["query"]);
                    ViewData["NoResults"] = GamesResponse == null ? "" : "No Games Matching The Query!";
				}
                catch (Exception ex)
                {
					_logger.LogError(ex, "Error occurred while fetching games.");
				}
			}
		}
    }
}
