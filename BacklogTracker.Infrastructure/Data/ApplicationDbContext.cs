using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BacklogTracker.Infrastructure.Entities;

namespace BacklogTracker.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<BacklogTrackerUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
