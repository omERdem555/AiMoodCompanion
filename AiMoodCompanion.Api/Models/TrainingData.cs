using System.ComponentModel.DataAnnotations;

namespace AiMoodCompanion.Api.Models
{
    public class TrainingData
    {
        public int Id { get; set; }
        
        [Required]
        public string InputText { get; set; } = string.Empty;
        
        [Required]
        public string DetectedMood { get; set; } = string.Empty;
        
        public double MoodScore { get; set; }
        
        public string? Keywords { get; set; }
        
        public string? UserFeedback { get; set; }
        
        public bool IsUsedForTraining { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UsedForTrainingAt { get; set; }
        
        // Navigation properties
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
