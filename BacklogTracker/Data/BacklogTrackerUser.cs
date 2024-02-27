using Microsoft.AspNetCore.Identity;

namespace BacklogTracker.Data
{
	public class BacklogTrackerUser : IdentityUser
	{
		public List<string>? GameIDs { get; set; }
	}
}
