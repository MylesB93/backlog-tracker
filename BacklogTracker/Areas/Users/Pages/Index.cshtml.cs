using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BacklogTracker.ViewModels;

namespace BacklogTracker.Areas.Users.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IUserService _userService;

		public List<User> Users { get; set; }

		public IndexModel(IUserService userService)
		{
			_userService = userService;
		}

		public void OnGet()
		{
			var users = _userService.GetAllUsers();
			if (users != null)
			{
				Users = users.Select(u => new User { Id = u.Id, UserName = u.UserName }).ToList();
			}
		}
	}
}
