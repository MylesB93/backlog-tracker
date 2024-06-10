using BacklogTracker.Data;
using BacklogTracker.Interfaces;
using BacklogTracker.Services;
using Moq;
using BacklogTracker.Controllers.API;
using Microsoft.AspNetCore.Mvc;

namespace BacklogTracker.Tests;

public class BacklogUpdateTests
{
	private readonly Mock<IBacklogService> _backlogServiceMock;
	private readonly BacklogController _controller;

	// For reference - https://code-maze.com/unit-testing-aspnetcore-web-api/
	public BacklogUpdateTests()
	{
		_backlogServiceMock = new Mock<IBacklogService>();
		_backlogServiceMock.Setup(s => s.GetBacklog("mylesbroomestest@hotmail.co.uk")).Returns(new List<string>() { "1234", "4567", "7890" });

		_controller = new BacklogController(_backlogServiceMock.Object);
	}

	[Fact]
	public void GetUsersBacklog_ReturnsListOfGameIds()
	{
		// Act
		var result = _controller.GetUsersBacklog("mylesbroomestest@hotmail.co.uk") as OkObjectResult;

		// Assert
		var gameIds = Assert.IsType<List<string>>(result?.Value);
		Assert.Equal(3, gameIds.Count);
	}
}