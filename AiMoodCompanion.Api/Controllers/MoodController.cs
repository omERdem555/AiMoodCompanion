using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AiMoodCompanion.Api.DTOs;
using AiMoodCompanion.Api.Services;

namespace AiMoodCompanion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodController : ControllerBase
    {
        private readonly MoodAnalysisService _moodService;

        public MoodController(MoodAnalysisService moodService)
        {
            _moodService = moodService;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<MoodAnalysisResponseDto>> AnalyzeMood([FromBody] MoodAnalysisRequestDto request)
        {
            try
            {
                var result = await _moodService.AnalyzeMoodAndGetRecommendationsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error analyzing mood: {ex.Message}");
            }
        }

        [HttpPost("analyze-anonymous")]
        public async Task<ActionResult<MoodAnalysisResponseDto>> AnalyzeMoodAnonymous([FromBody] MoodAnalysisRequestDto request)
        {
            try
            {
                // Ensure no user ID for anonymous analysis
                request.UserId = null;
                var result = await _moodService.AnalyzeMoodAndGetRecommendationsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error analyzing mood: {ex.Message}");
            }
        }
    }
}
