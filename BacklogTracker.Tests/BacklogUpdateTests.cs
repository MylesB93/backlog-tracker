using Moq;
using BacklogTracker.Application.Interfaces;
using BacklogTracker.Controllers.API;
using BacklogTracker.Application.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BacklogTracker.Tests;

public class BacklogUpdateTests
{
	private readonly Mock<IBacklogService> _backlogServiceMock;
	private readonly BacklogController _controller;

	private readonly UserDto _validUserDto;
	private readonly UserDto _userDtoWithoutEmail;
	private readonly UserDto _userDtoWithoutGameID;
	private readonly UserDto _nonExistentUserDto;
	private readonly UserDto _existingGameIDUserDto;
	private readonly UserDto _gameNotInBacklogUserDto;

	// For reference - https://code-maze.com/unit-testing-aspnetcore-web-api/
	public BacklogUpdateTests()
	{
		_backlogServiceMock = new Mock<IBacklogService>();
		_validUserDto = new UserDto() { Email = "mylesbroomestest@hotmail.co.uk", GameID = "1234" };
		_userDtoWithoutEmail = new UserDto() { Email = "", GameID = "1234" };
		_userDtoWithoutGameID = new UserDto() { Email = "mylesbroomestest@hotmail.co.uk", GameID = "" };
		_nonExistentUserDto = new UserDto() { Email = "nonexistentuser@test.com", GameID = "1234" };
		_existingGameIDUserDto = new UserDto() { Email = "mylesbroomestest@test.com", GameID = "0000" };
		_gameNotInBacklogUserDto = new UserDto() { Email = "mylesbroomestest@test.com", GameID = "9999" };

		// Setup GetBacklog() tests
		_backlogServiceMock.Setup(s => s.GetBacklog("mylesbroomestest@hotmail.co.uk")).Returns(new List<string>() { "1234", "4567", "7890" });
		_backlogServiceMock.Setup(s => s.GetBacklog("invalidemail@test.com")).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.GetBacklog("nogames@test.com")).Returns(new List<string>());

		// Setup AddToBacklog() tests
		_backlogServiceMock.Setup(s => s.AddToBacklog(_validUserDto));
		_backlogServiceMock.Setup(s => s.AddToBacklog(_userDtoWithoutEmail)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.AddToBacklog(_userDtoWithoutGameID)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.AddToBacklog(_nonExistentUserDto)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.AddToBacklog(_existingGameIDUserDto)).Throws(new ArgumentException());

		// Setup RemoveFromBacklog() tests
		_backlogServiceMock.Setup(s => s.RemoveFromBacklog(_validUserDto));
		_backlogServiceMock.Setup(s => s.RemoveFromBacklog(_userDtoWithoutEmail)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.RemoveFromBacklog(_userDtoWithoutGameID)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.RemoveFromBacklog(_nonExistentUserDto)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.RemoveFromBacklog(_gameNotInBacklogUserDto)).Throws(new Exception());

		// Setup AddToCompleted() tests
		_backlogServiceMock.Setup(s => s.AddToCompleted(_validUserDto));
		_backlogServiceMock.Setup(s => s.AddToCompleted(_userDtoWithoutEmail)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.AddToCompleted(_userDtoWithoutGameID)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.AddToCompleted(_nonExistentUserDto)).Throws(new Exception());
		//_backlogServiceMock.Setup(s => s.AddToCompleted(_existingGameIDUserDto)).Throws(new ArgumentException());

		// Setup RemoveFromCompleted() tests
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(_validUserDto));
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(_userDtoWithoutEmail)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(_userDtoWithoutGameID)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(_nonExistentUserDto)).Throws(new Exception());
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(_gameNotInBacklogUserDto)).Throws(new Exception());

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

	[Fact]
	public void AddToBacklog_WhenUserDoesntExist_Returns500StatusCode()
	{
		// Act
		var result = _controller.AddToBacklog(_nonExistentUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void AddToBacklog_WhenGameIDAlreadyExists_Returns409StatusCode()
	{
		// Act
		var result = _controller.AddToBacklog(_existingGameIDUserDto) as ObjectResult;

		// Assert
		Assert.Equal(409, result?.StatusCode);
	}

	[Fact]
	public void RemoveFromBacklog_WhenCalled_Returns200StatusCode()
	{
		// Act
		var result = _controller.RemoveFromBacklog(_validUserDto) as OkObjectResult;

		// Assert
		Assert.Equal(200, result?.StatusCode);
	}

	[Fact]
	public void RemoveFromBacklog_WhenUserDtoHasNoEmail_Returns500StatusCode()
	{
		// Act
		var result = _controller.RemoveFromBacklog(_userDtoWithoutEmail) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void RemoveFromBacklog_WhenUserDtoHasNoGameID_Returns500StatusCode()
	{
		// Act
		var result = _controller.RemoveFromBacklog(_userDtoWithoutGameID) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void RemoveFromBacklog_WhenUserDoesntExist_Returns500StatusCode()
	{
		// Act
		var result = _controller.RemoveFromBacklog(_nonExistentUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void RemoveFromBacklog_WhenGameIDNotInBacklog_Returns500StatusCode()
	{
		// Act
		var result = _controller.RemoveFromBacklog(_gameNotInBacklogUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void AddToCompleted_WhenCalled_Returns200StatusCode()
	{
		// Act
		var result = _controller.AddToCompleted(_validUserDto) as OkObjectResult;

		// Assert
		Assert.Equal(200, result?.StatusCode);
	}

	[Fact]
	public void AddToCompleted_WhenUserDtoHasNoEmail_Returns500StatusCode()
	{
		// Act
		var result = _controller.AddToCompleted(_userDtoWithoutEmail) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void AddToCompleted_WhenUserDtoHasNoGameID_Returns500StatusCode()
	{
		// Act
		var result = _controller.AddToCompleted(_userDtoWithoutGameID) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void AddToCompleted_WhenUserDoesntExist_Returns500StatusCode()
	{
		// Act
		var result = _controller.AddToCompleted(_nonExistentUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenCalled_Returns200StatusCode()
	{
		// Act
		var result = _controller.MoveFromCompletedToBacklog(_validUserDto) as OkObjectResult;

		// Assert
		Assert.Equal(200, result?.StatusCode);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenUserDtoHasNoEmail_Returns500StatusCode()
	{
		// Act
		var result = _controller.MoveFromCompletedToBacklog(_userDtoWithoutEmail) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenUserDtoHasNoGameID_Returns500StatusCode()
	{
		// Act
		var result = _controller.MoveFromCompletedToBacklog(_userDtoWithoutGameID) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenUserDoesntExist_Returns500StatusCode()
	{
		// Act
		var result = _controller.MoveFromCompletedToBacklog(_nonExistentUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenGameNotInCompleted_Returns500StatusCode()
	{
		// Act
		var result = _controller.MoveFromCompletedToBacklog(_gameNotInBacklogUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	// Response body assertions

	[Fact]
	public void GetUsersBacklog_WhenCalled_ReturnsCorrectGameIds()
	{
		// Act
		var result = _controller.GetUsersBacklog("mylesbroomestest@hotmail.co.uk") as OkObjectResult;

		// Assert
		var gameIds = Assert.IsType<List<string>>(result?.Value);
		Assert.Contains("1234", gameIds);
		Assert.Contains("4567", gameIds);
		Assert.Contains("7890", gameIds);
	}

	[Fact]
	public void AddToBacklog_WhenCalled_ReturnsExpectedMessage()
	{
		// Act
		var result = _controller.AddToBacklog(_validUserDto) as OkObjectResult;

		// Assert
		Assert.NotNull(result?.Value);
		var message = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value)?.ToString();
		Assert.Equal("Data saved successfully.", message);
	}

	[Fact]
	public void RemoveFromBacklog_WhenCalled_ReturnsExpectedMessage()
	{
		// Act
		var result = _controller.RemoveFromBacklog(_validUserDto) as OkObjectResult;

		// Assert
		Assert.NotNull(result?.Value);
		var message = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value)?.ToString();
		Assert.Equal("Data saved successfully.", message);
	}

	[Fact]
	public void AddToCompleted_WhenCalled_ReturnsExpectedMessage()
	{
		// Act
		var result = _controller.AddToCompleted(_validUserDto) as OkObjectResult;

		// Assert
		Assert.NotNull(result?.Value);
		var message = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value)?.ToString();
		Assert.Equal("Data saved successfully.", message);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenCalled_ReturnsExpectedMessage()
	{
		// Act
		var result = _controller.MoveFromCompletedToBacklog(_validUserDto) as OkObjectResult;

		// Assert
		Assert.NotNull(result?.Value);
		var message = result.Value.GetType().GetProperty("Message")?.GetValue(result.Value)?.ToString();
		Assert.Equal("Data saved successfully.", message);
	}

	// Verify service method invocations

	[Fact]
	public void AddToBacklog_WhenCalled_InvokesServiceExactlyOnce()
	{
		// Act
		_controller.AddToBacklog(_validUserDto);

		// Assert
		_backlogServiceMock.Verify(s => s.AddToBacklog(_validUserDto), Times.Once);
	}

	[Fact]
	public void RemoveFromBacklog_WhenCalled_InvokesServiceExactlyOnce()
	{
		// Act
		_controller.RemoveFromBacklog(_validUserDto);

		// Assert
		_backlogServiceMock.Verify(s => s.RemoveFromBacklog(_validUserDto), Times.Once);
	}

	[Fact]
	public void AddToCompleted_WhenCalled_InvokesServiceExactlyOnce()
	{
		// Act
		_controller.AddToCompleted(_validUserDto);

		// Assert
		_backlogServiceMock.Verify(s => s.AddToCompleted(_validUserDto), Times.Once);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenCalled_InvokesRemoveFromCompletedAndAddToBacklog()
	{
		// Act
		_controller.MoveFromCompletedToBacklog(_validUserDto);

		// Assert
		_backlogServiceMock.Verify(s => s.RemoveFromCompleted(_validUserDto), Times.Once);
		_backlogServiceMock.Verify(s => s.AddToBacklog(_validUserDto), Times.Once);
	}

	[Fact]
	public void GetUsersBacklog_WhenCalled_InvokesServiceExactlyOnce()
	{
		// Act
		_controller.GetUsersBacklog("mylesbroomestest@hotmail.co.uk");

		// Assert
		_backlogServiceMock.Verify(s => s.GetBacklog("mylesbroomestest@hotmail.co.uk"), Times.Once);
	}

	// Error message assertions

	[Fact]
	public void GetUsersBacklog_WhenUserDoesntExist_ReturnsErrorMessageInBody()
	{
		// Act
		var result = _controller.GetUsersBacklog("invalidemail@test.com") as ObjectResult;

		// Assert
		Assert.NotNull(result?.Value);
		Assert.Contains("An error occurred", result.Value.ToString());
	}

	[Fact]
	public void AddToBacklog_WhenGameIDAlreadyExists_ReturnsConflictMessage()
	{
		// Act
		var result = _controller.AddToBacklog(_existingGameIDUserDto) as ObjectResult;

		// Assert
		Assert.Equal(409, result?.StatusCode);
		Assert.NotNull(result?.Value);
	}

	// GetBacklog returns null scenario

	[Fact]
	public void GetUsersBacklog_WhenServiceReturnsNull_ReturnsEmptyList()
	{
		// Arrange
		_backlogServiceMock.Setup(s => s.GetBacklog("nullbacklog@test.com")).Returns((List<string>?)null);

		// Act
		var result = _controller.GetUsersBacklog("nullbacklog@test.com") as OkObjectResult;

		// Assert
		var gameIds = Assert.IsType<List<string>>(result?.Value);
		Assert.Empty(gameIds);
	}

	// Test for null UserDto
	[Fact]
	public void AddToBacklog_WhenUserDtoIsNull_LetsCatchBlockHandle_Returns500StatusCode()
	{
		// Arrange
		_backlogServiceMock.Setup(s => s.AddToBacklog(null!)).Throws(new NullReferenceException());

		// Act
		var result = _controller.AddToBacklog(null!) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void RemoveFromBacklog_WhenUserDtoIsNull_LetsCatchBlockHandle_Returns500StatusCode()
	{
		// Arrange
		_backlogServiceMock.Setup(s => s.RemoveFromBacklog(null!)).Throws(new NullReferenceException());

		// Act
		var result = _controller.RemoveFromBacklog(null!) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void AddToCompleted_WhenUserDtoIsNull_LetsCatchBlockHandle_Returns500StatusCode()
	{
		// Arrange
		_backlogServiceMock.Setup(s => s.AddToCompleted(null!)).Throws(new NullReferenceException());

		// Act
		var result = _controller.AddToCompleted(null!) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenUserDtoIsNull_LetsCatchBlockHandle_Returns500StatusCode()
	{
		// Arrange
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(null!)).Throws(new NullReferenceException());

		// Act
		var result = _controller.MoveFromCompletedToBacklog(null!) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
	}

	// Test for partial failure in MoveFromCompletedToBacklog
	[Fact]
	public void MoveFromCompletedToBacklog_WhenRemoveFromCompletedFails_Returns500StatusCode()
	{
		// Arrange - Setup RemoveFromCompleted to throw but AddToBacklog to succeed
		var testUserDto = new UserDto() { Email = "partial@test.com", GameID = "7777" };
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(testUserDto)).Throws(new Exception("Cannot remove"));
		_backlogServiceMock.Setup(s => s.AddToBacklog(testUserDto));

		// Act
		var result = _controller.MoveFromCompletedToBacklog(testUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
		// Verify that AddToBacklog was never called because RemoveFromCompleted threw first
		_backlogServiceMock.Verify(s => s.AddToBacklog(testUserDto), Times.Never);
	}

	[Fact]
	public void MoveFromCompletedToBacklog_WhenAddToBacklogFailsAfterRemove_Returns500StatusCode()
	{
		// Arrange - RemoveFromCompleted succeeds but AddToBacklog fails
		var testUserDto = new UserDto() { Email = "partial2@test.com", GameID = "8888" };
		_backlogServiceMock.Setup(s => s.RemoveFromCompleted(testUserDto));
		_backlogServiceMock.Setup(s => s.AddToBacklog(testUserDto)).Throws(new Exception("Cannot add to backlog"));

		// Act
		var result = _controller.MoveFromCompletedToBacklog(testUserDto) as ObjectResult;

		// Assert
		Assert.Equal(500, result?.StatusCode);
		// Verify both methods were called
		_backlogServiceMock.Verify(s => s.RemoveFromCompleted(testUserDto), Times.Once);
		_backlogServiceMock.Verify(s => s.AddToBacklog(testUserDto), Times.Once);
	}

	// Test that GetUsersBacklog verifies correct email is passed
	[Fact]
	public void GetUsersBacklog_PassesCorrectEmailToService()
	{
		// Arrange
		string testEmail = "specific@test.com";
		_backlogServiceMock.Setup(s => s.GetBacklog(testEmail)).Returns(new List<string>());

		// Act
		_controller.GetUsersBacklog(testEmail);

		// Assert
		_backlogServiceMock.Verify(s => s.GetBacklog(testEmail), Times.Once);
		_backlogServiceMock.Verify(s => s.GetBacklog(It.IsAny<string>()), Times.Once);
	}

	// Test ArgumentException is caught and returns 409 Conflict
	[Fact]
	public void AddToBacklog_WhenArgumentExceptionThrown_Returns409StatusCode()
	{
		// Arrange
		var conflictUserDto = new UserDto() { Email = "conflict@test.com", GameID = "3333" };
		_backlogServiceMock.Setup(s => s.AddToBacklog(conflictUserDto)).Throws(new ArgumentException("Duplicate"));

		// Act
		var result = _controller.AddToBacklog(conflictUserDto) as ObjectResult;

		// Assert
		Assert.Equal(409, result?.StatusCode);
	}
}