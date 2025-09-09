namespace AiMoodCompanion.Api.DTOs
{
    public class MoodAnalysisRequestDto
    {
        public string InputText { get; set; } = string.Empty;
        public int? UserId { get; set; }
    }

    public class MoodAnalysisResponseDto
    {
        public string DetectedMood { get; set; } = string.Empty;
        public double MoodScore { get; set; }
        public List<string> Keywords { get; set; } = new List<string>();
        public List<RecommendationDto> Recommendations { get; set; } = new List<RecommendationDto>();
    }

    public class RecommendationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int? Year { get; set; }
        public string? ImageUrl { get; set; }
        public string? ExternalId { get; set; }
    }

    public class UserReactionDto
    {
        public int RecommendationId { get; set; }
        public string ReactionType { get; set; } = string.Empty; // Like, Dislike, WatchLater
    }
}
