using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Areas.Identity.Pages.Account.Manage
{
    public class GamesModel : PageModel
    {
        private readonly IGameService _gameService;
        public GamesModel(IGameService gameService) 
        {
            _gameService = gameService;
        }
        public void OnGet()
        {
            // TODO: Get user's games from Backlog API then call service to get games from GiantBomb API
        }
    }
}
