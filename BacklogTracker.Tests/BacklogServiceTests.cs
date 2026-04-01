using Moq;
using BacklogTracker.Application.Data.DTOs;
using BacklogTracker.Application.Interfaces;
using BacklogTracker.Services;

namespace BacklogTracker.Tests;

public class BacklogServiceTests
{
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly BacklogService _backlogService;

	private readonly UserDto _validUserDto;

	public BacklogServiceTests()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_validUserDto = new UserDto() { Email = "test@example.com", GameID = "1234" };
		_backlogService = new BacklogService(_userRepositoryMock.Object);
	}

	// AddToBacklog tests
	[Fact]
	public void AddToBacklog_DelegatesToRepository()
	{
		// Act
		_backlogService.AddToBacklog(_validUserDto);

		// Assert
		_userRepositoryMock.Verify(r => r.AddToUsersBacklog(_validUserDto), Times.Once);
	}

	[Fact]
	public void AddToBacklog_PropagatesException()
	{
		// Arrange
		var exception = new ArgumentException("User not found");
		_userRepositoryMock.Setup(r => r.AddToUsersBacklog(_validUserDto)).Throws(exception);

		// Act & Assert
		var thrownException = Assert.Throws<ArgumentException>(() => _backlogService.AddToBacklog(_validUserDto));
		Assert.Equal("User not found", thrownException.Message);
	}

	// RemoveFromBacklog tests
	[Fact]
	public void RemoveFromBacklog_DelegatesToRepository()
	{
		// Act
		_backlogService.RemoveFromBacklog(_validUserDto);

		// Assert
		_userRepositoryMock.Verify(r => r.RemoveFromUsersBacklog(_validUserDto), Times.Once);
	}

	[Fact]
	public void RemoveFromBacklog_PropagatesException()
	{
		// Arrange
		var exception = new Exception("Game not in backlog");
		_userRepositoryMock.Setup(r => r.RemoveFromUsersBacklog(_validUserDto)).Throws(exception);

		// Act & Assert
		var thrownException = Assert.Throws<Exception>(() => _backlogService.RemoveFromBacklog(_validUserDto));
		Assert.Equal("Game not in backlog", thrownException.Message);
	}

	// GetBacklog tests
	[Fact]
	public void GetBacklog_ReturnsList_FromRepository()
	{
		// Arrange
		var expectedGameIds = new List<string> { "1234", "5678", "9999" };
		_userRepositoryMock.Setup(r => r.GetUsersBacklog("test@example.com")).Returns(expectedGameIds);

		// Act
		var result = _backlogService.GetBacklog("test@example.com");

		// Assert
		Assert.Equal(expectedGameIds, result);
		_userRepositoryMock.Verify(r => r.GetUsersBacklog("test@example.com"), Times.Once);
	}

	[Fact]
	public void GetBacklog_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
	{
		// Arrange
		var emptyList = new List<string>();
		_userRepositoryMock.Setup(r => r.GetUsersBacklog("test@example.com")).Returns(emptyList);

		// Act
		var result = _backlogService.GetBacklog("test@example.com");

		// Assert
		Assert.NotNull(result);
		Assert.Empty(result);
	}

	[Fact]
	public void GetBacklog_ReturnsNull_WhenRepositoryReturnsNull()
	{
		// Arrange
		_userRepositoryMock.Setup(r => r.GetUsersBacklog("test@example.com")).Returns((List<string>?)null);

		// Act
		var result = _backlogService.GetBacklog("test@example.com");

		// Assert
		Assert.Null(result);
	}

	// AddToCompleted tests
	[Fact]
	public void AddToCompleted_DelegatesToRepository()
	{
		// Act
		_backlogService.AddToCompleted(_validUserDto);

		// Assert
		_userRepositoryMock.Verify(r => r.AddToCompleted(_validUserDto), Times.Once);
	}

	[Fact]
	public void AddToCompleted_PropagatesException()
	{
		// Arrange
		var exception = new InvalidOperationException("Cannot add to completed");
		_userRepositoryMock.Setup(r => r.AddToCompleted(_validUserDto)).Throws(exception);

		// Act & Assert
		var thrownException = Assert.Throws<InvalidOperationException>(() => _backlogService.AddToCompleted(_validUserDto));
		Assert.Equal("Cannot add to completed", thrownException.Message);
	}

	// GetCompleted tests
	[Fact]
	public void GetCompleted_ReturnsList_FromRepository()
	{
		// Arrange
		var expectedGameIds = new List<string> { "1111", "2222" };
		_userRepositoryMock.Setup(r => r.GetUsersCompletedGames("test@example.com")).Returns(expectedGameIds);

		// Act
		var result = _backlogService.GetCompleted("test@example.com");

		// Assert
		Assert.Equal(expectedGameIds, result);
		_userRepositoryMock.Verify(r => r.GetUsersCompletedGames("test@example.com"), Times.Once);
	}

	[Fact]
	public void GetCompleted_ReturnsEmpty_WhenRepositoryReturnsEmpty()
	{
		// Arrange
		_userRepositoryMock.Setup(r => r.GetUsersCompletedGames("test@example.com")).Returns(new List<string>());

		// Act
		var result = _backlogService.GetCompleted("test@example.com");

		// Assert
		Assert.NotNull(result);
		Assert.Empty(result);
	}

	// RemoveFromCompleted tests
	[Fact]
	public void RemoveFromCompleted_DelegatesToRepository()
	{
		// Act
		_backlogService.RemoveFromCompleted(_validUserDto);

		// Assert
		_userRepositoryMock.Verify(r => r.RemoveFromUsersCompleted(_validUserDto), Times.Once);
	}

	[Fact]
	public void RemoveFromCompleted_PropagatesException()
	{
		// Arrange
		var exception = new Exception("Game not in completed");
		_userRepositoryMock.Setup(r => r.RemoveFromUsersCompleted(_validUserDto)).Throws(exception);

		// Act & Assert
		var thrownException = Assert.Throws<Exception>(() => _backlogService.RemoveFromCompleted(_validUserDto));
		Assert.Equal("Game not in completed", thrownException.Message);
	}
}
