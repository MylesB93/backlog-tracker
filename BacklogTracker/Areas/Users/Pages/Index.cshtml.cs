using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BacklogTracker.ViewModels;

namespace BacklogTracker.Areas.Users.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IUserRepository _userRepository;

		public List<User> Users { get; set; }

		public IndexModel(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public void OnGet()
		{
			var users = _userRepository.GetAllUsers();
			if (users != null)
			{
				Users = users.Select(u => new User { Id = u.Id, UserName = u.UserName }).ToList();
			}
		}
	}
}
