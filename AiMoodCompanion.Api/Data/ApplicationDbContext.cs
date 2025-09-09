using Microsoft.EntityFrameworkCore;
using AiMoodCompanion.Api.Models;

namespace AiMoodCompanion.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<UserReaction> UserReactions { get; set; }
        public DbSet<MoodAnalysis> MoodAnalyses { get; set; }
        public DbSet<TrainingData> TrainingData { get; set; }
        public DbSet<ModelVersions> ModelVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Recommendation configuration
            modelBuilder.Entity<Recommendation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Type).IsRequired();
            });

            // UserReaction configuration
            modelBuilder.Entity<UserReaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ReactionType).IsRequired();
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserReactions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Recommendation)
                    .WithMany(r => r.UserReactions)
                    .HasForeignKey(e => e.RecommendationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MoodAnalysis configuration
            modelBuilder.Entity<MoodAnalysis>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InputText).IsRequired();
                entity.Property(e => e.DetectedMood).IsRequired();
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Seed data for recommendations
            SeedRecommendations(modelBuilder);
        }

        private void SeedRecommendations(ModelBuilder modelBuilder)
        {
            var recommendations = new List<Recommendation>
            {
                new Recommendation
                {
                    Id = 1,
                    Title = "The Shawshank Redemption",
                    Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    Type = "Movie",
                    Genre = "Drama",
                    Year = 1994,
                    ImageUrl = "https://example.com/shawshank.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 2,
                    Title = "The Lord of the Rings",
                    Description = "A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring.",
                    Type = "Movie",
                    Genre = "Fantasy",
                    Year = 2001,
                    ImageUrl = "https://example.com/lotr.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 3,
                    Title = "1984",
                    Description = "A dystopian novel about a totalitarian society where Big Brother is always watching.",
                    Type = "Book",
                    Genre = "Dystopian",
                    Year = 1949,
                    ImageUrl = "https://example.com/1984.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 4,
                    Title = "Breaking Bad",
                    Description = "A high school chemistry teacher turned methamphetamine manufacturer partners with a former student.",
                    Type = "Series",
                    Genre = "Crime",
                    Year = 2008,
                    ImageUrl = "https://example.com/breaking-bad.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 5,
                    Title = "The Grand Budapest Hotel",
                    Description = "A legendary concierge and his young protégé embark on a series of adventures.",
                    Type = "Movie",
                    Genre = "Comedy",
                    Year = 2014,
                    ImageUrl = "https://example.com/grand-budapest.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 6,
                    Title = "La La Land",
                    Description = "A jazz pianist falls for an aspiring actress in Los Angeles.",
                    Type = "Movie",
                    Genre = "Romance",
                    Year = 2016,
                    ImageUrl = "https://example.com/lalaland.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 7,
                    Title = "Mad Max: Fury Road",
                    Description = "A woman rebels against a tyrannical ruler in post-apocalyptic Australia.",
                    Type = "Movie",
                    Genre = "Action",
                    Year = 2015,
                    ImageUrl = "https://example.com/madmax.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 8,
                    Title = "The Conjuring",
                    Description = "Paranormal investigators Ed and Lorraine Warren work to help a family terrorized by a dark presence.",
                    Type = "Movie",
                    Genre = "Horror",
                    Year = 2013,
                    ImageUrl = "https://example.com/conjuring.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 9,
                    Title = "Planet Earth II",
                    Description = "Documentary series exploring the natural world and its wonders.",
                    Type = "Series",
                    Genre = "Documentary",
                    Year = 2016,
                    ImageUrl = "https://example.com/planetearth.jpg",
                    CreatedAt = DateTime.UtcNow
                },
                new Recommendation
                {
                    Id = 10,
                    Title = "The Martian",
                    Description = "An astronaut becomes stranded on Mars and must find a way to survive.",
                    Type = "Movie",
                    Genre = "Adventure",
                    Year = 2015,
                    ImageUrl = "https://example.com/martian.jpg",
                    CreatedAt = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<Recommendation>().HasData(recommendations);
        }
    }
}
