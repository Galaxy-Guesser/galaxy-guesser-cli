using GalaxyGuesserApi.Data;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;
using GalaxyGuesserApi.Repositories.Interfaces;

namespace GalaxyGuesserApi.Repositories
{
  public class PlayerRepository : IPlayerRepository
  {
    private readonly DatabaseContext _dbContext;

    public PlayerRepository(DatabaseContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
      const string query = @"
            SELECT player_id, user_name, guid
            FROM players";

      return await _dbContext.QueryAsync(query, reader => new Player
      {
        playerId = reader.GetInt32(0),
        userName = reader.GetString(1),
        guid = reader.GetString(2)
      });
    }

    public async Task<Player> GetPlayerByIdAsync(int playerId)
    {
      const string query = @"
            SELECT player_id, user_name, guid
            FROM players
            WHERE player_id = @playerId";

      var parameters = new Dictionary<string, object> { { "@playerId", playerId } };

      var result = await _dbContext.QueryAsync(query, reader => new Player
      {
        playerId = reader.GetInt32(0),
        userName = reader.GetString(1),
        guid = reader.GetString(2)
      }, parameters);

      return result.FirstOrDefault()!;
    }

    public async Task<Player> GetPlayerByGuidAsync(string guid)
    {
      const string query = @"
            SELECT player_id, user_name, guid
            FROM players
            WHERE guid = @guid";

      var parameters = new Dictionary<string, object> { { "@guid", guid } };

      var result = await _dbContext.QueryAsync(query, reader => new Player
      {
        playerId = reader.GetInt32(0),
        userName = reader.GetString(1),
        guid = reader.GetString(2)
      }, parameters);

      return result.FirstOrDefault()!;
    }

    public async Task<Player> GetUserByGoogleIdAsync(string guid)
    {
      const string query = @"
                SELECT player_id, user_name, guid
                FROM players
                WHERE guid = @guid";

      var parameters = new Dictionary<string, object> { { "@guid", guid } };

      var players = await _dbContext.QueryAsync(query, reader => new Player
      {
        playerId = reader.GetInt32(0),
        userName = reader.GetString(1),
        guid = reader.GetString(2)
      }, parameters);

      return players.FirstOrDefault();
    }

    public async Task<Player> CreatePlayerAsync(string guid, string userName)
    {
      const string sql = @"
            INSERT INTO players (user_name, guid)
            VALUES (@userName, @guid)
            RETURNING player_id, user_name, guid";
      var parameters = new Dictionary<string, object>
            {
                { "@userName", userName },
                { "@guid", guid}
            };

      var result = await _dbContext.QueryAsync(sql, reader => new Player
      {
        playerId = reader.GetInt32(0),
        userName = reader.GetString(1),
        guid = reader.GetString(2)
      }, parameters);

      return result.First();

    }

    public async Task<bool> UpdatePlayerUsernameAsync(int playerId, string userName)
    {
      const string query = @"
            UPDATE players
            SET user_name = @userName
            WHERE player_id = @playerId";

      var parameters = new Dictionary<string, object>
            {
                { "@playerId", playerId },
                { "@userName", userName }
            };

      var affectedRows = await _dbContext.ExecuteAsync(query, parameters);
      return affectedRows > 0;
    }

    public async Task<bool> DeletePlayerAsync(int playerId)
    {
      const string query = @"
            DELETE FROM players
            WHERE player_id = @playerId";

      var parameters = new Dictionary<string, object> { { "@playerId", playerId } };

      var affectedRows = await _dbContext.ExecuteAsync(query, parameters);
      return affectedRows > 0;
    }

    public async Task<List<PlayerStatsDTO>> GetPlayersStats(int playerId)
    {
      const string sql = @"SELECT DISTINCT ON (sessions.session_code)
    sessions.session_code,
    c.category,
    s.score,
    RANK() OVER (PARTITION BY s.session_id ORDER BY s.score DESC) AS rank,
    MAX(s.score) OVER () AS overall_highest_score,
    total.total_sessions
FROM sessions
LEFT JOIN categories c ON sessions.category_id = c.category_id
LEFT JOIN players p ON sessions.created_by = p.player_id
LEFT JOIN sessionscores s ON sessions.session_id = s.session_id
CROSS JOIN (
    SELECT COUNT(*) AS total_sessions FROM sessions
) AS total
WHERE s.player_id = @playerId
ORDER BY sessions.session_code, s.score DESC
		  ";

      var parameters = new Dictionary<string, object> { { "@playerId", playerId } };

      var sessions = await _dbContext.QueryAsync(sql, reader => new PlayerStatsDTO
      {
        sessionCode = reader.GetString(0),
        category = reader.GetString(1),
        sessionScore = reader.GetInt32(2),
        ranking = reader.GetInt32(3),
        totalSessions = reader.GetInt32(4)
      }, parameters);

      return sessions.ToList();
    }
  }
}