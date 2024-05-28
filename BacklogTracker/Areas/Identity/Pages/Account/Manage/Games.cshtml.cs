using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BacklogTracker.Areas.Identity.Pages.Account.Manage
{
    public class GamesModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IBacklogService _backlogService;

        public Response Backlog { get; set; }
        public Response CompletedGames { get; set; }

        public GamesModel(IGameService gameService, IBacklogService backlogService) 
        {
            _gameService = gameService;
            _backlogService = backlogService;
            Backlog = new Response();
            CompletedGames = new Response();
        }
        public async Task OnGet()
        {
            var email = User?.Identity?.Name;

            var gameIds = email != null ? _backlogService.GetBacklog(email) : new List<string>();
            var completedGameIds = email != null ? _backlogService.GetCompleted(email) : new List<string>();
            
            if (gameIds != null && gameIds.Any())
            {
                Backlog = await _gameService.GetUsersGamesAsync(gameIds);
            }

			if (completedGameIds != null && completedGameIds.Any())
			{
				CompletedGames = await _gameService.GetUsersGamesAsync(completedGameIds);
			}
		}
    }
}
