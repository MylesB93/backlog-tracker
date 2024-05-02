using BacklogTracker.Data;
using BacklogTracker.Services;
using Moq;

namespace BacklogTracker.Tests;

public class BacklogUpdateTests
{
    [Fact]
    public void GetUsersBacklog_SingleUser_ReturnsAllIDsInBacklog()
    {
        var mockDbContext = new Mock<ApplicationDbContext>();
        var expectedUser = new BacklogTrackerUser { Id = "1", Email = "testuser@test.com", GameIDs = new List<string>() { "1234", "5678", "9012" } };
        mockDbContext.Setup(db => db.Users.FirstOrDefault(u => u.Id == "1")).Returns(expectedUser);

        var backlogService = new BacklogService(mockDbContext.Object);

        var backlog = backlogService.GetBacklog(expectedUser.Email);

		Assert.All(new List<string>() { "1234", "5678", "9012" }, item => Assert.Contains(item, backlog));
	}
}