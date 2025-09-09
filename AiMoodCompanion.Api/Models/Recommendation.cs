using System.ComponentModel.DataAnnotations;

namespace AiMoodCompanion.Api.Models
{
    public class Recommendation
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Type { get; set; } = string.Empty; // Movie, Book, Series
        
        public string? Genre { get; set; }
        
        public int? Year { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public string? ExternalId { get; set; } // IMDB, ISBN, etc.
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<UserReaction> UserReactions { get; set; } = new List<UserReaction>();
    }
}
