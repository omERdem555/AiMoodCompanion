using AiMoodCompanion.Api.Data;
using AiMoodCompanion.Api.DTOs;
using AiMoodCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AiMoodCompanion.Api.Services
{
    public class MoodAnalysisService
    {
        private readonly ApplicationDbContext _context;

        public MoodAnalysisService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MoodAnalysisResponseDto> AnalyzeMoodAndGetRecommendationsAsync(MoodAnalysisRequestDto request)
        {
            // Simple mood analysis based on keywords
            var moodResult = AnalyzeMood(request.InputText);
            
            // Save mood analysis to database
            var moodAnalysis = new MoodAnalysis
            {
                InputText = request.InputText,
                UserId = request.UserId,
                DetectedMood = moodResult.Mood,
                MoodScore = moodResult.Score,
                Keywords = System.Text.Json.JsonSerializer.Serialize(moodResult.Keywords)
            };

            _context.MoodAnalyses.Add(moodAnalysis);
            await _context.SaveChangesAsync();

            // Get recommendations based on mood
            var recommendations = await GetRecommendationsByMoodAsync(moodResult.Mood, moodResult.Keywords);

            return new MoodAnalysisResponseDto
            {
                DetectedMood = moodResult.Mood,
                MoodScore = moodResult.Score,
                Keywords = moodResult.Keywords,
                Recommendations = recommendations
            };
        }

        private (string Mood, double Score, List<string> Keywords) AnalyzeMood(string inputText)
        {
            var text = inputText.ToLower();
            var keywords = new List<string>();
            double rawScore = 0.0;

            // Pozitif, negatif ve nötr kelimeler
            var positiveWords = new[] { "iyiyim", "mutluyum", "harika", "güzel", "seviniyorum", "heyecanlı", "enerjik", "neşeli", "keyifli" };
            var negativeWords = new[] { "kötüyüm", "üzgünüm", "yorgunum", "stresli", "endişeli", "korkuyorum", "sinirli", "mutsuz", "depresif" };
            var neutralWords = new[] { "normal", "sakin", "durgun", "nötr", "fark etmez", "bilmiyorum" };

            foreach (var word in positiveWords)
            {
                if (text.Contains(word))
                {
                    keywords.Add(word);
                    rawScore += 0.3;
                }
            }

            foreach (var word in negativeWords)
            {
                if (text.Contains(word))
                {
                    keywords.Add(word);
                    rawScore -= 0.3;
                }
            }

            foreach (var word in neutralWords)
            {
                if (text.Contains(word))
                {
                    keywords.Add(word);
                    // rawScore değişmiyor
                }
            }

            // rawScore aralığı yaklaşık -2.7 ile +2.7 arasında olabilir (9 kelime * 0.3)
            double minScore = -2.7;
            double maxScore = 2.7;

            // Normalize et (0-1 aralığına)
            double normalizedScore = (rawScore - minScore) / (maxScore - minScore);

            // 0-10 aralığına ölçekle
            double scaledScore = normalizedScore * 10;

            // Mood belirle (0-10’a göre eşikler)
            string mood;
            if (scaledScore >= 7)
                mood = "Happy";
            else if (scaledScore <= 3)
                mood = "Sad";
            else
                mood = "Neutral";

            return (mood, scaledScore, keywords);
        }

        private async Task<List<RecommendationDto>> GetRecommendationsByMoodAsync(string mood, List<string> keywords)
        {
            var query = _context.Recommendations.AsQueryable();

            // Filter recommendations based on mood
            switch (mood.ToLower())
            {
                case "happy":
                    query = query.Where(r => r.Genre == "Comedy" || r.Genre == "Adventure" || r.Genre == "Fantasy");
                    break;
                case "sad":
                    query = query.Where(r => r.Genre == "Drama" || r.Genre == "Romance" || r.Genre == "Inspirational");
                    break;
                case "excited":
                    query = query.Where(r => r.Genre == "Action" || r.Genre == "Thriller" || r.Genre == "Adventure");
                    break;
                case "relaxed":
                    query = query.Where(r => r.Genre == "Documentary" || r.Genre == "Nature" || r.Genre == "Meditation");
                    break;
                default:
                    // For neutral mood, return diverse recommendations
                    break;
            }

            var recommendations = await query.Take(10).ToListAsync();

            return recommendations.Select(r => new RecommendationDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Type = r.Type,
                Genre = r.Genre,
                Year = r.Year,
                ImageUrl = r.ImageUrl,
                ExternalId = r.ExternalId
            }).ToList();
        }
    }
}
