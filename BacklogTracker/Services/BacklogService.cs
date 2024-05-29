using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;
using BacklogTracker.Interfaces;

namespace BacklogTracker.Services
{
    public class BacklogService : IBacklogService
    {
        private readonly IUserRepository _userRepository;
        public BacklogService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddToBacklog(UserDto userDto)
        {
            _userRepository.AddToUsersBacklog(userDto);
        }

        public void RemoveFromBacklog(UserDto userDto)
        {
            _userRepository?.RemoveFromUsersBacklog(userDto);
        }

        public List<string>? GetBacklog(string email)
        {
            return _userRepository.GetUsersBacklog(email);
        }

        public void AddToCompleted(UserDto userDto) 
        {
            _userRepository.AddToCompleted(userDto);
        }

		public List<string>? GetCompleted(string email)
		{
			return _userRepository.GetUsersCompletedGames(email);
		}

        public void RemoveFromCompleted(UserDto userDto)
        {

        }
	}
}
