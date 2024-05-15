using BacklogTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BacklogTracker.Areas.Users.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public List<string> Users { get; set; }

        public IndexModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet()
        {
            Users = _userRepository.GetAllUsers().Select(u => u.UserName).ToList();
        }
    }
}
