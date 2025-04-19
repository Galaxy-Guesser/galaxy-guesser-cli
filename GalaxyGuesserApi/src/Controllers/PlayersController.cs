using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GalaxyGuesserApi.Models;
using GalaxyGuesserApi.Models.DTO;
using System.Security.Claims;
using GalaxyGuesserApi.Services;
using System.ComponentModel.DataAnnotations;
using GalaxyGuesserApi.Models.DTO;

namespace GalaxyGuesserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        }

        /// <summary>
        /// Gets the Google ID from the current user's claims
        /// </summary>
        /// <returns>The Google ID if found, null otherwise</returns>
        private string? GetGoogleIdFromClaims()
        {
            return User.FindFirst("sub")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Validates that the current user is authenticated with a valid Google ID
        /// </summary>
        /// <returns>An unauthorized result if not authenticated, null if authenticated</returns>
        private ActionResult<ApiResponse<T>>? ValidateAuthentication<T>()
        {
            var googleId = GetGoogleIdFromClaims();
            if (string.IsNullOrEmpty(googleId))
            {
                return Unauthorized(ApiResponse<T>.ErrorResponse("User not authenticated"));
            }
            return null;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Player>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Player>>>> GetPlayers()
        {
            try
            {
                var players = await _playerService.GetAllPlayersAsync();
                return Ok(ApiResponse<IEnumerable<Player>>.SuccessResponse(players, "Players retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<IEnumerable<Player>>.ErrorResponse("An error occurred while retrieving players", new List<string> { ex.Message }));
            }
        }

        [HttpGet("{playerId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Player>>> GetPlayer([Required] int playerId)
        {
            try
            {
                var unauthorizedResult = ValidateAuthentication<Player>();
                if (unauthorizedResult != null) return unauthorizedResult;

                var googleId = GetGoogleIdFromClaims();
                var player = await _playerService.GetPlayerByGuidAsync(googleId);
                
                if (player == null)
                {
                    return NotFound(ApiResponse<Player>.ErrorResponse($"Player with ID {playerId} not found"));
                }
                
                return Ok(ApiResponse<Player?>.SuccessResponse(player, "Player retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<Player>.ErrorResponse("An error occurred while retrieving the player", new List<string> { ex.Message }));
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Player>>> CreatePlayer([FromBody] CreatePlayerRequest request)
        {
            try
            {
                var unauthorizedResult = ValidateAuthentication<Player>();
                if (unauthorizedResult != null) return unauthorizedResult;

                var googleId = GetGoogleIdFromClaims();
            
                if (string.IsNullOrWhiteSpace(request.UserName))
                {
                    return BadRequest(ApiResponse<Player>.ErrorResponse("Username is required"));
                }

                var player = await _playerService.CreatePlayerAsync(googleId, request.UserName);
                
                return CreatedAtAction(nameof(GetPlayer), new { player.playerId }, 
                    ApiResponse<Player>.SuccessResponse(player, "Player created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<Player>.ErrorResponse("An error occurred while creating the player", new List<string> { ex.Message }));
            }
        }

        [HttpPut("{playerId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Player>>> UpdatePlayer([Required] int playerId, [FromBody] UpdatePlayerRequest request)
        {
            try
            {
                var unauthorizedResult = ValidateAuthentication<Player>();
                if (unauthorizedResult != null) return unauthorizedResult;

                if (string.IsNullOrWhiteSpace(request.UserName))
                {
                    return BadRequest(ApiResponse<Player>.ErrorResponse("Username is required"));
                }

                var googleId = GetGoogleIdFromClaims();
                var existingPlayer = await _playerService.GetPlayerByGuidAsync(googleId);

                if (existingPlayer == null)
                {
                    return NotFound(ApiResponse<Player>.ErrorResponse($"Player with ID {playerId} not found"));
                }

                if (existingPlayer?.guid != googleId)
                {
                    return Forbid();
                }

                var updated = await _playerService.UpdatePlayerUsernameAsync(playerId, request.UserName);

                if (!updated)
                {
                    return NotFound(ApiResponse<Player>.ErrorResponse($"Player with ID {playerId} not found"));
                }

                return Ok(ApiResponse<Player?>.SuccessResponse(existingPlayer, "Player updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<Player>.ErrorResponse("An error occurred while updating the player", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("{playerId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Player>>> DeletePlayer([Required] int playerId)
        {
            try
            {
                var unauthorizedResult = ValidateAuthentication<Player>();
                if (unauthorizedResult != null) return unauthorizedResult;

                var googleId = GetGoogleIdFromClaims();
                var existingPlayer = await _playerService.GetPlayerByGuidAsync(googleId);

                if (existingPlayer == null)
                {
                    return NotFound(ApiResponse<Player>.ErrorResponse($"Player with ID {playerId} not found"));
                }

                if (existingPlayer?.guid != googleId)
                {
                    return Forbid();
                }

                var deleted = await _playerService.DeletePlayerAsync(playerId);

                if (!deleted)
                {
                    return NotFound(ApiResponse<Player>.ErrorResponse($"Player with ID {playerId} not found"));
                }

                return Ok(ApiResponse<Player?>.SuccessResponse(existingPlayer, "Player deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<Player>.ErrorResponse("An error occurred while deleting the player", new List<string> { ex.Message }));
            }
        }

        [Authorize]
        [HttpPost("auth")]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Player>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<Player>>> AuthenticateOrRegister([FromBody] string displayName)
        {
            try
            {
                var unauthorizedResult = ValidateAuthentication<Player>();
                if (unauthorizedResult != null) return unauthorizedResult;

                var googleId = GetGoogleIdFromClaims();
                var player = await _playerService.GetPlayerByGuidAsync(googleId);

                if (player == null)
                {
                    if (string.IsNullOrWhiteSpace(displayName))
                    {
                        return BadRequest(ApiResponse<Player>.ErrorResponse("Display name is required for new player registration"));
                    }

                    player = await _playerService.CreatePlayerAsync(googleId, displayName);
                    return Ok(ApiResponse<Player?>.SuccessResponse(player, "Player registered successfully"));
                }

                return Ok(ApiResponse<Player?>.SuccessResponse(player, "Player authenticated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    ApiResponse<Player>.ErrorResponse("An error occurred while authenticating or registering the player", new List<string> { ex.Message }));
            }
        }

        [HttpGet("{playerId}/stats")]
    public async Task<ActionResult<IEnumerable<PlayerStatsDTO>>> GetPlayersStats(int playerId)
    {
      try
      {
        var profileSessions = await _playerService.GetPlayersStats(playerId);
        return Ok(profileSessions);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Error: {ex.Message}");
      }
    }
    }
    }