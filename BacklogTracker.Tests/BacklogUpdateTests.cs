using BacklogTracker.Data;
using BacklogTracker.Interfaces;
using BacklogTracker.Services;
using Moq;
using BacklogTracker.Controllers.API;
using Microsoft.AspNetCore.Mvc;
using BacklogTracker.Data.DTOs;

namespace BacklogTracker.Tests;

public class BacklogUpdateTests
{
	private readonly Mock<IBacklogService> _backlogServiceMock;
	private readonly BacklogController _controller;
	private readonly UserDto _validUserDto;
	private readonly UserDto _userDtoWithoutEmail;
	private readonly UserDto _userDtoWithoutGameID;

	// For reference - https://code-maze.com/unit-testing-aspnetcore-web-api/
	public BacklogUpdateTests()
	{
		_backlogServiceMock = new Mock<IBacklogService>();
		_validUserDto = new UserDto() { Email = "mylesbroomestest@hotmail.co.uk", GameID = "1234" };
		_userDtoWithoutEmail = new UserDto() { Email = "", GameID = "1234" };
		_userDtoWithoutGameID = new UserDto() { Email = "mylesbroomestest@hotmail.co.uk", GameID = "" };

		// Setup GetBacklog() tests
		_backlogServiceMock.Setup(s => s.GetBacklog("mylesbroomestest@hotmail.co.uk")).Returns(new List<string>() { "1234", "4567", "7890" });
		_backlogServiceMock.Setup(s => s.GetBacklog("invalidemail@test.com")).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.GetBacklog("nogames@test.com")).Returns(new List<string>());

		// Setup AddToBacklog() tests
		_backlogServiceMock.Setup(s => s.AddToBacklog(_validUserDto));
		_backlogServiceMock.Setup(s => s.AddToBacklog(_userDtoWithoutEmail)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.AddToBacklog(_userDtoWithoutGameID)).Throws(new Exception());

		_controller = new BacklogController(_backlogServiceMock.Object);
	}

	[Fact]
	public void GetUsersBacklog_WhenCalled_ReturnsListOfGameIds()
	{
		// Act
		var result = _controller.GetUsersBacklog("mylesbroomestest@hotmail.co.uk") as OkObjectResult;

		// Assert
		var gameIds = Assert.IsType<List<string>>(result?.Value);
		Assert.Equal(3, gameIds.Count);
	}

	[Fact]
	public void GetUsersBacklog_WhenUserDoesntExist_Returns500StatusCode()
	{
		// Act
		var result = _controller.GetUsersBacklog("invalidemail@test.com") as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void GetUsersBacklog_WhenUserHasNoGames_ReturnsEmptyList()
	{
		// Act
		var result = _controller.GetUsersBacklog("nogames@test.com") as OkObjectResult;

		// Assert
		var gameIds = Assert.IsType<List<string>>(result?.Value);
		Assert.Empty(gameIds);
	}

	[Fact]
	public void AddToBacklog_WhenCalled_Returns200StatusCode()
	{
		// Act
		var result = _controller.AddToBacklog(_validUserDto) as OkObjectResult;

		// Assert
		Assert.Equal(200, result?.StatusCode);
	}

	[Fact]
	public void AddToBacklog_WhenUserDtoHasNoEmail_Returns500StatusCode()
	{
		// Act
		var result = _controller.AddToBacklog(_userDtoWithoutEmail) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void AddToBacklog_WhenUserDtoHasNoGameID_Returns500StatusCode()
	{
		// Act
		var result = _controller.AddToBacklog(_userDtoWithoutGameID) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}
}