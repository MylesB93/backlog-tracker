using BacklogTracker.Data;
using BacklogTracker.Interfaces;
using BacklogTracker.Services;
using Moq;

namespace BacklogTracker.Tests;

public class BacklogUpdateTests
{
    [Fact]
    public void GetUsersBacklog_SingleUser_ReturnsAllIDsInBacklog()
    {
        var mockUserRepo = new Mock<IUserRepository>();
        //var backlogService = new BacklogService(ApplicationDbContext dbContext) - move dbContext logic to user repository?
	}
}