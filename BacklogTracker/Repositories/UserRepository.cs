using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;
using BacklogTracker.Interfaces;

namespace BacklogTracker.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _dbContext;
		public UserRepository(ApplicationDbContext dbContext) 
		{ 
			_dbContext = dbContext;
		}
		public BacklogTrackerUser? GetUser(string email)
		{
			return _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();
		}
	}
}
