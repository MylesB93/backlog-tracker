﻿using BacklogTracker.Data;
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
		
		public void AddToUsersBacklog(UserDto userDto)
		{
			if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
			{
				throw new ArgumentException("GameID and Email are required.");
			}

			var user = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
			if (user == null)
			{
				throw new ArgumentException("User not found.");
			}

			var gameIDs = user.GameIDs;

			if (gameIDs.Contains(userDto.GameID))
			{
				throw new ArgumentException("GameID already exists in the user's backlog.");
			}

			gameIDs.Add(userDto.GameID);
			_dbContext.SaveChanges();
		}

		public void RemoveFromUsersBacklog(UserDto userDto) 
		{
			if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
			{
				throw new ArgumentException("GameID and Email are required.");
			}

			var user = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
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

		public List<string>? GetUsersBacklog(string email)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
			if (user == null)
			{
				throw new ArgumentException("User not found.");
			}

			var backlog = user.GameIDs;

			return backlog;
		}

		public List<BacklogTrackerUser> GetAllUsers()
		{
			return _dbContext.Users.ToList();
		}

		public BacklogTrackerUser? GetUser(string? id)
		{
			return _dbContext.Users.Where(u => u.Id == id).FirstOrDefault();
		}

		public void AddToCompleted(UserDto userDto)
		{
			if (string.IsNullOrEmpty(userDto.GameID) || string.IsNullOrEmpty(userDto.Email))
			{
				throw new ArgumentException("GameID and Email are required.");
			}

			var user = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
			if (user == null)
			{
				throw new ArgumentException("User not found.");
			}

			var gameIDs = user.GameIDs;
			var completedGameIDs = user.CompletedGameIDs;

			if (gameIDs.Contains(userDto.GameID) && !completedGameIDs.Contains(userDto.GameID))
			{
				completedGameIDs.Add(userDto.GameID);
				gameIDs.Remove(userDto.GameID);
				_dbContext.SaveChanges();
			}			
		}
	}
}
