using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Areas.Identity.Pages.Account.Manage
{
	public class GamesModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IBacklogService _backlogService;
        private readonly ILogger<GamesModel> _logger;

        public Response Backlog { get; set; }
        public Response CompletedGames { get; set; }

        public GamesModel(IGameService gameService, IBacklogService backlogService, ILogger<GamesModel> logger) 
        {
            _gameService = gameService;
            _backlogService = backlogService;
            Backlog = new Response();
            CompletedGames = new Response();
            _logger = logger;
        }
        public async Task OnGet()
        {
            var email = User?.Identity?.Name;

            var gameIds = email != null ? _backlogService.GetBacklog(email) : new List<string>();
            var completedGameIds = email != null ? _backlogService.GetCompleted(email) : new List<string>();
            
            if (gameIds != null && gameIds.Any())
            {
                _logger.LogInformation("Fetching backlog...");
                Backlog = await _gameService.GetUsersGamesAsync(gameIds);
            }

			if (completedGameIds != null && completedGameIds.Any())
			{
                _logger.LogInformation("Fetching completed games...");
				CompletedGames = await _gameService.GetUsersGamesAsync(completedGameIds);
			}
		}
    }
}
