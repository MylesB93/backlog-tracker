﻿using Microsoft.AspNetCore.Identity;

namespace BacklogTracker.Data
{
	public class BacklogTrackerUser : IdentityUser
	{
		public List<string>? GameIDs { get; set; }
		public List<string>? CompletedGameIDs { get; set; }

		public BacklogTrackerUser() 
		{ 
			GameIDs = new List<string>();
			CompletedGameIDs = new List<string>();
		}
	}
}
