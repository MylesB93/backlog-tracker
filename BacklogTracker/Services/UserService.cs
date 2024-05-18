using BacklogTracker.Data;
using BacklogTracker.Interfaces;

namespace BacklogTracker.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		public UserService(IUserRepository userRepository) 
		{
			_userRepository = userRepository;
		}

		public List<BacklogTrackerUser> GetAllUsers()
		{
			return _userRepository.GetAllUsers();
		}

		public BacklogTrackerUser? GetUser(string? id)
		{
			return _userRepository.GetUser(id);
		}
	}
}
