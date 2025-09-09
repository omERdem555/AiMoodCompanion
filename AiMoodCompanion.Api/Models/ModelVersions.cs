using System.ComponentModel.DataAnnotations;

namespace AiMoodCompanion.Api.Models
{
    public class ModelVersions
    {
        public int Id { get; set; }
        
        [Required]
        public string Version { get; set; } = string.Empty;
        
        [Required]
        public string ModelPath { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public double Accuracy { get; set; }
        
        public double Precision { get; set; }
        
        public double Recall { get; set; }
        
        public int TrainingDataCount { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? DeployedAt { get; set; }
        
        public bool IsActive { get; set; } = false;
        
        public string? Notes { get; set; }
    }
}
