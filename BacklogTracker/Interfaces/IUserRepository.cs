using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;

namespace BacklogTracker.Interfaces
{
	public interface IUserRepository
	{
		void AddToUsersBacklog(UserDto userDto);
		void RemoveFromUsersBacklog(UserDto userDto);
		List<string>? GetUsersBacklog(string email);
		List<BacklogTrackerUser> GetAllUsers();
	}
}
