using System.ComponentModel.DataAnnotations;

namespace AiMoodCompanion.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public int Age { get; set; }
        
        public string? ProfilePicture { get; set; }
        
        public string? Gender { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
        public virtual ICollection<UserReaction> UserReactions { get; set; } = new List<UserReaction>();
    }
}
