using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;
using BacklogTracker.Interfaces;

namespace BacklogTracker.Services
{
    public class BacklogService : IBacklogService
    {
        private readonly ApplicationDbContext _dbContext;
        public BacklogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddToBacklog(UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
            {
                throw new ArgumentException("GameID and Email are required.");
            }

            var user = _dbContext.Users.Where(u => u.Email == userDto.Email).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var gameIDs = user.GameIDs;

            if (gameIDs == null)
            {
                gameIDs = new List<string>();
            }

            if (gameIDs.Contains(userDto.GameID))
            {
                throw new ArgumentException("GameID already exists in the user's backlog.");
            }

            gameIDs.Add(userDto.GameID);
            _dbContext.SaveChanges();
        }

        public void RemoveFromBacklog(UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
            {
                throw new ArgumentException("GameID and Email are required.");
            }

            var user = _dbContext.Users.Where(u => u.Email == userDto.Email).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var gameIDs = user.GameIDs;

            if (gameIDs != null && gameIDs.Contains(userDto.GameID))
            {
                gameIDs.Remove(userDto.GameID);
                _dbContext.SaveChanges();
            }
        }

        public List<string>? GetBacklog(string email)
        {
            var user = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var backlog = user.GameIDs;

            return backlog;
        }


    }
}
