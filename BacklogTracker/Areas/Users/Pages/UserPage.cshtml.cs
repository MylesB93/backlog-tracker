using BacklogTracker.Data;
using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Areas.Users.Pages
{
    public class UserPageModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public List<string>? GamesList { get; set; }

        public UserPageModel(IUserService userService, IGameService gameService) 
        { 
            _userService = userService;
            _gameService = gameService;
        }

        public async void OnGet()
        {
            var userId = Request.Query["userId"];
            BacklogTrackerUser? user = new BacklogTrackerUser();

            if (!string.IsNullOrEmpty(userId)) 
            {
				user = _userService.GetUser(userId);
                var gameIds = user?.GameIDs;
                if (gameIds != null && gameIds.Any())
                {
					var userGamesList = await _gameService.GetUsersGamesAsync(gameIds);
                    GamesList = userGamesList.Games.Select(g => g.Name).ToList();
				}
			}            
        }
    }
}
