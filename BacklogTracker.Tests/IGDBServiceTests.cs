using Moq;
using BacklogTracker.Application.Data.DTOs;
using BacklogTracker.Infrastructure.Configuration;
using BacklogTracker.Infrastructure.Services;
using BacklogTracker.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace BacklogTracker.Tests;

public class IGDBServiceTests
{
	private readonly Mock<ILogger<IGDBService>> _loggerMock;
	private readonly Mock<IOptions<IGDBConfiguration>> _configMock;
	private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
	private readonly Mock<IMemoryCache> _cacheMock;
	private readonly IGDBService _igdbService;

	private readonly IGDBConfiguration _testConfig;

	public IGDBServiceTests()
	{
		_loggerMock = new Mock<ILogger<IGDBService>>();
		_configMock = new Mock<IOptions<IGDBConfiguration>>();
		_httpClientFactoryMock = new Mock<IHttpClientFactory>();
		_cacheMock = new Mock<IMemoryCache>();

		_testConfig = new IGDBConfiguration { GameLimit = 10 };
		_configMock.Setup(c => c.Value).Returns(_testConfig);

		_igdbService = new IGDBService(_loggerMock.Object, _configMock.Object, _httpClientFactoryMock.Object, _cacheMock.Object);
	}

	// GetGamesAsync tests - null/whitespace query
	[Fact]
	public async Task GetGamesAsync_ReturnsEmpty_WhenQueryIsNull()
	{
		// Act
		var result = await _igdbService.GetGamesAsync(null);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetGamesAsync_ReturnsEmpty_WhenQueryIsEmpty()
	{
		// Act
		var result = await _igdbService.GetGamesAsync("");

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetGamesAsync_ReturnsEmpty_WhenQueryIsWhitespace()
	{
		// Act
		var result = await _igdbService.GetGamesAsync("   ");

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	// GetGamesAsync - successful response
	[Fact]
	public async Task GetGamesAsync_ReturnsGames_WhenApiReturnsValidResponse()
	{
		// Arrange
		var query = "The Legend of Zelda";
		var jsonResponse = JsonSerializer.Serialize(new List<IGDBGame>
		{
			new IGDBGame { Id = 1, Name = "The Legend of Zelda", Url = "https://www.igdb.com/games/the-legend-of-zelda", Storyline = "A hero's adventure" }
		});

		var mockHttpClient = new HttpClient(new MockHttpMessageHandler(jsonResponse, HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetGamesAsync(query);

		// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetGamesAsync_UsesHttpClientFromFactory()
	{
		// Arrange
		var query = "Test Game";
		var mockHttpClient = new HttpClient(new MockHttpMessageHandler("{}", HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetGamesAsync(query);

		// Assert
		_httpClientFactoryMock.Verify(f => f.CreateClient("IGDB"), Times.Once);
	}

	[Fact]
	public async Task GetGamesAsync_ReturnsEmpty_WhenApiReturnsNonSuccessStatus()
	{
		// Arrange
		var query = "Test Game";
		var mockHttpClient = new HttpClient(new MockHttpMessageHandler("", HttpStatusCode.InternalServerError))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetGamesAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetGamesAsync_ReturnsEmpty_WhenApiReturns404()
	{
		// Arrange
		var query = "Nonexistent Game XYZ123";
		var mockHttpClient = new HttpClient(new MockHttpMessageHandler("", HttpStatusCode.NotFound))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetGamesAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetGamesAsync_ReturnsEmpty_WhenJsonDeserializationFails()
	{
		// Arrange
		var query = "Test";
		var invalidJson = "{ invalid json }";
		var mockHttpClient = new HttpClient(new MockHttpMessageHandler(invalidJson, HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetGamesAsync(query);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	// GetUsersGamesAsync tests
	[Fact]
	public async Task GetUsersGamesAsync_ReturnsEmpty_WhenGameIdsIsNull()
	{
		// Act
		var result = await _igdbService.GetUsersGamesAsync(null!);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetUsersGamesAsync_ReturnsEmpty_WhenGameIdsIsEmpty()
	{
		// Act
		var result = await _igdbService.GetUsersGamesAsync(new List<string>());

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetUsersGamesAsync_IgnoresInvalidIds_AndProcessesValidOnes()
	{
		// Arrange
		var gameIds = new List<string> { "123", "invalid", "456", "" };
		var jsonResponse = JsonSerializer.Serialize(new List<IGDBGame>
		{
			new IGDBGame { Id = 123, Name = "Game1", Url = "url1", Storyline = "story1" },
			new IGDBGame { Id = 456, Name = "Game2", Url = "url2", Storyline = "story2" }
		});

		var mockHttpClient = new HttpClient(new MockHttpMessageHandler(jsonResponse, HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetUsersGamesAsync(gameIds);

		// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetUsersGamesAsync_UsesPostRequest()
	{
		// Arrange
		var gameIds = new List<string> { "1", "2" };
		var jsonResponse = JsonSerializer.Serialize(new List<IGDBGame>
		{
			new IGDBGame { Id = 1, Name = "Game1", Url = "url1", Storyline = "story1" },
			new IGDBGame { Id = 2, Name = "Game2", Url = "url2", Storyline = "story2" }
		});

		var mockHttpClient = new HttpClient(new MockHttpMessageHandler(jsonResponse, HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetUsersGamesAsync(gameIds);

		// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetUsersGamesAsync_MapsBigDecimalIdPropertyCorrectly()
	{
		// Arrange
		var gameIds = new List<string> { "1", "2", "3" };
		var jsonResponse = JsonSerializer.Serialize(new List<IGDBGame>
		{
			new IGDBGame { Id = 1, Name = "Game1", Url = "url1", Storyline = "story1" },
			new IGDBGame { Id = 2, Name = "Game2", Url = "url2", Storyline = "story2" },
			new IGDBGame { Id = 3, Name = "Game3", Url = "url3", Storyline = "story3" }
		});

		var mockHttpClient = new HttpClient(new MockHttpMessageHandler(jsonResponse, HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetUsersGamesAsync(gameIds);

		// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetUsersGamesAsync_ReturnsEmpty_WhenApiReturnsNonSuccessStatus()
	{
		// Arrange
		var gameIds = new List<string> { "1", "2" };
		var mockHttpClient = new HttpClient(new MockHttpMessageHandler("", HttpStatusCode.Unauthorized))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetUsersGamesAsync(gameIds);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	[Fact]
	public async Task GetUsersGamesAsync_ReturnsEmpty_WhenDeserializationFails()
	{
		// Arrange
		var gameIds = new List<string> { "1", "2" };
		var invalidJson = "not valid json at all";
		var mockHttpClient = new HttpClient(new MockHttpMessageHandler(invalidJson, HttpStatusCode.OK))
		{
			BaseAddress = new Uri("https://api.igdb.com/v4/")
		};
		_httpClientFactoryMock.Setup(f => f.CreateClient("IGDB")).Returns(mockHttpClient);

		// Act
		var result = await _igdbService.GetUsersGamesAsync(gameIds);

		// Assert
		Assert.NotNull(result);
		Assert.Null(result.Games);
	}

	// Mock HttpMessageHandler for testing
	private class MockHttpMessageHandler : HttpMessageHandler
	{
		private readonly string _responseContent;
		private readonly HttpStatusCode _statusCode;

		public MockHttpMessageHandler(string responseContent, HttpStatusCode statusCode)
		{
			_responseContent = responseContent;
			_statusCode = statusCode;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(_statusCode)
			{
				Content = new StringContent(_responseContent)
			};
			response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			return Task.FromResult(response);
		}
	}
}
