using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;
using BacklogTracker.Interfaces;

namespace BacklogTracker.Services
{
    public class BacklogService : IBacklogService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        public BacklogService(ApplicationDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
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
    }
}
