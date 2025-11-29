using BacklogTracker.Application.Data.DTOs;

namespace BacklogTracker.Application.Interfaces
{
	public interface IUserService
	{
		List<UserDto> GetAllUsers();
		UserDto? GetUser(string? id);
	}
}
