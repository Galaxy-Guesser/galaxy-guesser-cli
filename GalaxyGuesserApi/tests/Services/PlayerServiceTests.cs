//using System.Collections.Generic;
//using System.Threading.Tasks;
//using GalaxyGuesserApi.Models;
//using GalaxyGuesserApi.Repositories.Interfaces;
//using GalaxyGuesserApi.Services;
//using Moq;
//using Xunit;

//namespace GalaxyGuesserApi.Tests.Services
//{
//  public class PlayerServiceTests
//  {
//    private readonly Mock<IPlayerRepository> _playerRepoMock;
//    private readonly PlayerService _playerService;

//    public PlayerServiceTests()
//    {
//      _playerRepoMock = new Mock<IPlayerRepository>();
//      _playerService = new PlayerService(_playerRepoMock.Object);
//    }

//    [Fact]
//    public async Task GetAllPlayersAsync_ReturnsPlayers()
//    {
//      var players = new List<Player>
//            {
//                new Player { playerId = 1, userName = "Alice", guid = "guid1" },
//                new Player { playerId = 2, userName = "Bob", guid = "guid2" }
//            };

//      _playerRepoMock.Setup(repo => repo.GetAllPlayersAsync()).ReturnsAsync(players);

//      var result = await _playerService.GetAllPlayersAsync();

//      Assert.Equal(2, result.Count);
//      Assert.Equal("Alice", result[0].userName);
//    }

//    [Fact]
//    public async Task GetPlayerByIdAsync_ReturnsPlayer()
//    {
//      var player = new Player { playerId = 1, userName = "Alice", guid = "guid1" };
//      _playerRepoMock.Setup(r => r.GetPlayerByIdAsync(1)).ReturnsAsync(player);

//      var result = await _playerService.GetPlayerByIdAsync(1);

//      Assert.Equal("Alice", result.userName);
//    }

//    [Fact]
//    public async Task GetPlayerByGuidAsync_ReturnsPlayer()
//    {
//      var player = new Player { playerId = 3, userName = "Charlie", guid = "abc123" };
//      _playerRepoMock.Setup(r => r.GetPlayerByGuidAsync("abc123")).ReturnsAsync(player);

//      var result = await _playerService.GetPlayerByGuidAsync("abc123");

//      Assert.Equal("Charlie", result?.userName);
//    }

//    [Fact]
//    public async Task CreatePlayerAsync_CreatesAndReturnsPlayer()
//    {
//      var player = new Player { playerId = 4, userName = "NewPlayer", guid = "newguid" };
//      _playerRepoMock.Setup(r => r.CreatePlayerAsync("newguid", "NewPlayer")).ReturnsAsync(player);

//      var result = await _playerService.CreatePlayerAsync("newguid", "NewPlayer");

//      Assert.Equal("NewPlayer", result.userName);
//    }

//    [Fact]
//    public async Task UpdatePlayerAsync_ReturnsTrue_WhenPlayerExists()
//    {
//      var player = new Player { playerId = 5, userName = "OldName", guid = "g123" };
//      _playerRepoMock.Setup(r => r.GetPlayerByIdAsync(5)).ReturnsAsync(player);
//      _playerRepoMock.Setup(r => r.UpdatePlayerAsync(5, "UpdatedName")).ReturnsAsync(true);

//      var result = await _playerService.UpdatePlayerAsync(5, "UpdatedName");

//      Assert.True(result);
//    }

//    [Fact]
//    public async Task UpdatePlayerAsync_ReturnsFalse_WhenPlayerNotFound()
//    {
//      _playerRepoMock.Setup(r => r.GetPlayerByIdAsync(99)).ReturnsAsync(default(Player));

//      var result = await _playerService.UpdatePlayerAsync(99, "Name");

//      Assert.False(result);
//    }

//    [Fact]
//    public async Task DeletePlayerAsync_ReturnsTrue_WhenPlayerExists()
//    {
//      var player = new Player { playerId = 6, userName = "DeleteMe", guid = "gdel" };
//      _playerRepoMock.Setup(r => r.GetPlayerByIdAsync(6)).ReturnsAsync(player);
//      _playerRepoMock.Setup(r => r.DeletePlayerAsync(6)).ReturnsAsync(true);

//      var result = await _playerService.DeletePlayerAsync(6);

//      Assert.True(result);
//    }

//    [Fact]
//    public async Task DeletePlayerAsync_ReturnsFalse_WhenPlayerNotFound()
//    {
//      _playerRepoMock.Setup(r => r.GetPlayerByIdAsync(100)).ReturnsAsync(default(Player));

//      var result = await _playerService.DeletePlayerAsync(100);

//      Assert.False(result);
//    }
//  }
//}