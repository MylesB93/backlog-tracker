using BacklogTracker.Application.Interfaces;
using BacklogTracker.Application.Data.DTOs;

namespace BacklogTracker.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		public UserService(IUserRepository userRepository) => _userRepository = userRepository;

		public List<UserDto> GetAllUsers() => _userRepository.GetAllUsers();

		public UserDto? GetUser(string? id) => _userRepository.GetUser(id);
	}
}
