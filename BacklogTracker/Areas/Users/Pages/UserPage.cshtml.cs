using BacklogTracker.Data;
using BacklogTracker.Interfaces;
using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Areas.Users.Pages
{
    public class UserPageModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly ILogger<UserPageModel> _logger;

        public List<Game>? GamesList { get; set; }
        public List<Game>? CompletedGamesList { get; set; }

        public UserPageModel(IUserService userService, IGameService gameService, ILogger<UserPageModel> logger) 
        { 
            _userService = userService;
            _gameService = gameService;
            _logger = logger;
        }

        public async Task OnGet()
        {
            var userId = Request.Query["userId"];
            BacklogTrackerUser? user = new BacklogTrackerUser();

            if (!string.IsNullOrEmpty(userId)) 
            {
                _logger.LogInformation("Fetching user...");
				user = _userService.GetUser(userId);

                var gameIds = user?.GameIDs;
                if (gameIds != null && gameIds.Any())
                {
                    _logger.LogInformation("Fetching user's games list...");
					var userGamesList = await _gameService.GetUsersGamesAsync(gameIds);
                    GamesList = userGamesList.Games?.ToList();
				}

                var completedGameIds = user?.CompletedGameIDs;
				if (completedGameIds != null && completedGameIds.Any())
				{
                    _logger.LogInformation("Fetching user's completed games list...");
					var userCompGamesList = await _gameService.GetUsersGamesAsync(completedGameIds);
					CompletedGamesList = userCompGamesList.Games?.ToList();
				}
			}            
        }
    }
}
