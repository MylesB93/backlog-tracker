using BacklogTracker.Application.Data.DTOs;

namespace BacklogTracker.Application.Interfaces
{
	public interface IUserRepository
	{
		void AddToUsersBacklog(UserDto userDto);
		void RemoveFromUsersBacklog(UserDto userDto);
		List<string>? GetUsersBacklog(string email);
		List<UserDto> GetAllUsers();
		UserDto? GetUser(string? id);
		void AddToCompleted(UserDto userDto);
		List<string>? GetUsersCompletedGames(string email);
		void RemoveFromUsersCompleted(UserDto userDto);
	}
}
