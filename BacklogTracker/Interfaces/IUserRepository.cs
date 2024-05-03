using BacklogTracker.Data;
using BacklogTracker.Data.DTOs;

namespace BacklogTracker.Interfaces
{
	public interface IUserRepository
	{
		BacklogTrackerUser? GetUser(string email);
	}
}
