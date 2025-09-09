using System.ComponentModel.DataAnnotations;

namespace AiMoodCompanion.Api.Models
{
    public class UserReaction
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int RecommendationId { get; set; }
        
        [Required]
        public string ReactionType { get; set; } = string.Empty; // Like, Dislike, WatchLater
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Recommendation Recommendation { get; set; } = null!;
    }
}
