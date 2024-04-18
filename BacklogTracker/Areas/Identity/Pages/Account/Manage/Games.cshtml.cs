using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Areas.Identity.Pages.Account.Manage
{
    public class GamesModel : PageModel
    {
        private readonly IGameService _gameService;
        private readonly IBacklogService _backlogService;
        public GamesModel(IGameService gameService, IBacklogService backlogService) 
        {
            _gameService = gameService;
            _backlogService = backlogService;
        }
        public void OnGet()
        {
            // Get list of user's backlog game ID's from backlog service
            // User that list to get list of BacklogGame from game service
        }
    }
}
