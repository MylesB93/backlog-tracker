using Moq;
using BacklogTracker.Application.Data.DTOs;
using BacklogTracker.Application.Interfaces;
using BacklogTracker.Services;

namespace BacklogTracker.Tests;

public class UserServiceTests
{
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly UserService _userService;

	public UserServiceTests()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_userService = new UserService(_userRepositoryMock.Object);
	}

	// GetAllUsers tests
	[Fact]
	public void GetAllUsers_ReturnsListOfUsers_FromRepository()
	{
		// Arrange
		var expectedUsers = new List<UserDto>
		{
			new UserDto { Id = "1", Email = "user1@example.com", UserName = "User1" },
			new UserDto { Id = "2", Email = "user2@example.com", UserName = "User2" }
		};
		_userRepositoryMock.Setup(r => r.GetAllUsers()).Returns(expectedUsers);

		// Act
		var result = _userService.GetAllUsers();

		// Assert
		Assert.Equal(2, result.Count);
		Assert.Contains(expectedUsers[0], result);
		_userRepositoryMock.Verify(r => r.GetAllUsers(), Times.Once);
	}

	[Fact]
	public void GetAllUsers_ReturnsEmptyList_WhenNoUsersExist()
	{
		// Arrange
		_userRepositoryMock.Setup(r => r.GetAllUsers()).Returns(new List<UserDto>());

		// Act
		var result = _userService.GetAllUsers();

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void GetAllUsers_PropagatesException_FromRepository()
	{
		// Arrange
		var exception = new Exception("Database connection failed");
		_userRepositoryMock.Setup(r => r.GetAllUsers()).Throws(exception);

		// Act & Assert
		var thrownException = Assert.Throws<Exception>(() => _userService.GetAllUsers());
		Assert.Equal("Database connection failed", thrownException.Message);
	}

	// GetUser tests
	[Fact]
	public void GetUser_ReturnsUser_WhenIdExists()
	{
		// Arrange
		var expectedUser = new UserDto { Id = "1", Email = "user1@example.com", UserName = "User1" };
		_userRepositoryMock.Setup(r => r.GetUser("1")).Returns(expectedUser);

		// Act
		var result = _userService.GetUser("1");

		// Assert
		Assert.NotNull(result);
		Assert.Equal("user1@example.com", result.Email);
		_userRepositoryMock.Verify(r => r.GetUser("1"), Times.Once);
	}

	[Fact]
	public void GetUser_ReturnsNull_WhenIdDoesNotExist()
	{
		// Arrange
		_userRepositoryMock.Setup(r => r.GetUser("999")).Returns((UserDto?)null);

		// Act
		var result = _userService.GetUser("999");

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void GetUser_ReturnsNull_WhenIdIsNull()
	{
		// Arrange
		_userRepositoryMock.Setup(r => r.GetUser(null)).Returns((UserDto?)null);

		// Act
		var result = _userService.GetUser(null);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void GetUser_PropagatesException_FromRepository()
	{
		// Arrange
		var exception = new Exception("Database query failed");
		_userRepositoryMock.Setup(r => r.GetUser("1")).Throws(exception);

		// Act & Assert
		var thrownException = Assert.Throws<Exception>(() => _userService.GetUser("1"));
		Assert.Equal("Database query failed", thrownException.Message);
	}
}
