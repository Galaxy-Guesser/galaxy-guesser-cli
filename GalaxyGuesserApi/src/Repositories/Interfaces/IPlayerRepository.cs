using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;

namespace GalaxyGuesserApi.Repositories.Interfaces
{
  public interface IPlayerRepository
  {
    Task<List<Player>> GetAllPlayersAsync();
    Task<Player> GetPlayerByIdAsync(int playerId);
    Task<Player> GetUserByGoogleIdAsync(string guid);
    Task<Player> CreatePlayerAsync(string guid, string userName);
    Task<bool> UpdatePlayerUsernameAsync(int playerId, string userName);
    Task<bool> DeletePlayerAsync(int playerId);
    Task<Player> GetPlayerByGuidAsync(string guid);
    Task<List<PlayerStatsDTO>> GetPlayersStats(int playerId);
  }
}