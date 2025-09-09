using System.ComponentModel.DataAnnotations;

namespace AiMoodCompanion.Api.Models
{
    public class MoodAnalysis
    {
        public int Id { get; set; }
        
        public int? UserId { get; set; } // Null for anonymous users
        
        [Required]
        public string InputText { get; set; } = string.Empty;
        
        [Required]
        public string DetectedMood { get; set; } = string.Empty; // Happy, Sad, Excited, etc.
        
        public double MoodScore { get; set; } // -1.0 to 1.0
        
        public string? Keywords { get; set; } // JSON array of extracted keywords
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User? User { get; set; }
    }
}
