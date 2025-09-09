using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AiMoodCompanion.Api.Data;
using AiMoodCompanion.Api.DTOs;
using AiMoodCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AiMoodCompanion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                ProfilePicture = user.ProfilePicture,
                Gender = user.Gender,
                CreatedAt = user.CreatedAt
            };
        }

        [HttpPost("reaction")]
        public async Task<ActionResult> AddReaction([FromBody] UserReactionDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Check if reaction already exists
            var existingReaction = await _context.UserReactions
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RecommendationId == request.RecommendationId);

            if (existingReaction != null)
            {
                // Update existing reaction
                existingReaction.ReactionType = request.ReactionType;
                existingReaction.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                // Create new reaction
                var reaction = new UserReaction
                {
                    UserId = userId,
                    RecommendationId = request.RecommendationId,
                    ReactionType = request.ReactionType,
                    CreatedAt = DateTime.UtcNow
                };
                _context.UserReactions.Add(reaction);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("reactions")]
        public async Task<ActionResult<List<RecommendationDto>>> GetUserReactions([FromQuery] string reactionType = "Like")
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var reactions = await _context.UserReactions
                .Where(r => r.UserId == userId && r.ReactionType == reactionType)
                .Include(r => r.Recommendation)
                .ToListAsync();

            var recommendations = reactions.Select(r => new RecommendationDto
            {
                Id = r.Recommendation.Id,
                Title = r.Recommendation.Title,
                Description = r.Recommendation.Description,
                Type = r.Recommendation.Type,
                Genre = r.Recommendation.Genre,
                Year = r.Recommendation.Year,
                ImageUrl = r.Recommendation.ImageUrl,
                ExternalId = r.Recommendation.ExternalId
            }).ToList();

            return Ok(recommendations);
        }

        [HttpGet("watchlist")]
        public async Task<ActionResult<List<RecommendationDto>>> GetWatchlist()
        {
            return await GetUserReactions("WatchLater");
        }

        [HttpGet("liked")]
        public async Task<ActionResult<List<RecommendationDto>>> GetLikedItems()
        {
            return await GetUserReactions("Like");
        }
    }
}
