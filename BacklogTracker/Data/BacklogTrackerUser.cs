using Microsoft.AspNetCore.Identity;

namespace BacklogTracker.Data
{
	public class BacklogTrackerUser : IdentityUser
	{
		public List<Guid> GameIDs { get; set; }
	}
}
