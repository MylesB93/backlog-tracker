﻿using BacklogTracker.Data;

namespace BacklogTracker.Interfaces
{
	public interface IUserService
	{
		List<BacklogTrackerUser> GetAllUsers();
		BacklogTrackerUser? GetUser(string? id);
	}
}
