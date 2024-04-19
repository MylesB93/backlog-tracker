using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using BacklogTracker.Models.UserBacklog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BacklogTracker.Areas.Identity.Pages.Account.Manage
{
    public class GamesModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IBacklogService _backlogService;

        public List<Response> Backlog { get; set; }

        public GamesModel(IGameService gameService, IBacklogService backlogService) 
        {
            _gameService = gameService;
            _backlogService = backlogService;
        }
        public async void OnGet()
        {
            var email = User?.Identity?.Name;

            // Get list of user's backlog game ID's from backlog service
            var gameIds = email != null ? _backlogService.GetBacklog(email) : new List<string>();          

            // User that list to get list of BacklogGame from game service
            // TODO: Backlog = gameIds != null ? await _gameService.GetUsersGamesAsync(gameIds) : new List<Response>();
        }
    }
}
