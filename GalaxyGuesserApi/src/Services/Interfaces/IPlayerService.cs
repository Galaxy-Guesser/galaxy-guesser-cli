using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;

namespace GalaxyGuesserApi.Services
{
  public interface IPlayerService
  {
    Task<List<Player>> GetAllPlayersAsync();
    Task<Player> GetPlayerByIdAsync(int player_id);
    Task<Player> CreatePlayerAsync(string guid, string username);
    Task<bool> UpdatePlayerUsernameAsync(int player_id, string username);
    Task<Player?> GetPlayerByGuidAsync(string guid);
    Task<bool> DeletePlayerAsync(int player_id);
    Task<List<PlayerStatsDTO>> GetPlayersStats(int playerId);
    }
}