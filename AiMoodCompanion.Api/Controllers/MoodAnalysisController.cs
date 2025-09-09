using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AiMoodCompanion.Api.Data;
using AiMoodCompanion.Api.Models;
using AiMoodCompanion.Api.Services;
using System.Text.Json;

namespace AiMoodCompanion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodAnalysisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MLMoodService _mlService;

        public MoodAnalysisController(ApplicationDbContext context, MLMoodService mlService)
        {
            _context = context;
            _mlService = mlService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeMood([FromBody] MoodAnalysisRequest request)
        {
            try
            {
                // ML.NET ile mood analizi
                var detectedMood = _mlService.PredictMood(request.InputText);
                var moodScore = CalculateMoodScore(request.InputText);
                var keywords = ExtractKeywords(request.InputText);

                // Mood analizi kaydet
                var moodAnalysis = new MoodAnalysis
                {
                    InputText = request.InputText,
                    DetectedMood = detectedMood,
                    MoodScore = moodScore,
                    Keywords = keywords,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.MoodAnalyses.Add(moodAnalysis);
                await _context.SaveChangesAsync();

                // Mood'a göre öneriler getir
                var recommendations = await GetRecommendationsByMood(detectedMood, request.UserId);

                // Training data olarak kaydet
                var trainingData = new TrainingData
                {
                    InputText = request.InputText,
                    DetectedMood = detectedMood,
                    MoodScore = moodScore,
                    Keywords = keywords,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TrainingData.Add(trainingData);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    DetectedMood = detectedMood,
                    MoodScore = moodScore,
                    Keywords = keywords,
                    Recommendations = recommendations,
                    AnalysisId = moodAnalysis.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations([FromQuery] string? mood = null, [FromQuery] int? userId = null)
        {
            try
            {
                var recommendations = await GetRecommendationsByMood(mood, userId);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Model eğitimi endpoint'i
        [HttpPost("train")]
        public async Task<IActionResult> TrainModel()
        {
            try
            {
                var success = await _mlService.TrainModelAsync();
                if (success)
                {
                    return Ok(new { Message = "Model başarıyla eğitildi!", Timestamp = DateTime.UtcNow });
                }
                else
                {
                    return BadRequest(new { Error = "Model eğitimi başarısız oldu." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Model durumu endpoint'i
        [HttpGet("model-status")]
        public async Task<IActionResult> GetModelStatus()
        {
            try
            {
                var modelVersions = await _context.ModelVersions
                    .OrderByDescending(mv => mv.CreatedAt)
                    .Take(5)
                    .Select(mv => new
                    {
                        mv.Version,
                        mv.Description,
                        mv.Accuracy,
                        mv.TrainingDataCount,
                        mv.CreatedAt,
                        mv.IsActive
                    })
                    .ToListAsync();

                return Ok(new
                {
                    ActiveModels = modelVersions.Count(mv => mv.IsActive),
                    TotalModels = modelVersions.Count,
                    LatestModel = modelVersions.FirstOrDefault(),
                    AllModels = modelVersions
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        private double CalculateMoodScore(string text)
        {
            var lowerText = text.ToLower();
            var score = 0.5; // Neutral başlangıç
            
            // Güçlendirici kelimeler (çok, çok çok, mükemmel, harika)
            var intensifiers = new[] { "çok", "çok çok", "mükemmel", "harika", "muhteşem", "olağanüstü", "fantastik" };
            var intensifierCount = 0;
            foreach (var word in intensifiers)
            {
                if (lowerText.Contains(word))
                {
                    intensifierCount++;
                    score += 0.15; // Her güçlendirici +0.15
                }
            }
            
            // Zayıflatıcı kelimeler (biraz, az, kısmen)
            var weakeners = new[] { "biraz", "az", "kısmen", "biraz", "azıcık" };
            foreach (var word in weakeners)
            {
                if (lowerText.Contains(word))
                {
                    score -= 0.1; // Her zayıflatıcı -0.1
                }
            }
            
            // Olumsuz ifadeler (hiç, değil, asla)
            var negators = new[] { "hiç", "değil", "asla", "hiçbir", "yok" };
            var hasNegator = negators.Any(word => lowerText.Contains(word));
            
            // Positive keywords
            var positiveWords = new[] { "mutlu", "güzel", "harika", "mükemmel", "sevgi", "love", "beautiful", "amazing", "keyif", "neşe" };
            foreach (var word in positiveWords)
            {
                if (lowerText.Contains(word))
                {
                    if (hasNegator)
                        score -= 0.2; // Olumsuz + pozitif = negatif
                    else
                        score += 0.1; // Sadece pozitif
                }
            }
            
            // Negative keywords
            var negativeWords = new[] { "üzgün", "keder", "korku", "öfke", "sad", "fear", "anger", "terrible", "hüzün", "endişe" };
            foreach (var word in negativeWords)
            {
                if (lowerText.Contains(word))
                {
                    if (hasNegator)
                        score += 0.2; // Olumsuz + negatif = pozitif
                    else
                        score -= 0.1; // Sadece negatif
                }
            }
            
            // Çok çok gibi tekrarlanan kelimeler için ekstra bonus
            if (intensifierCount >= 2)
            {
                score += 0.1; // Çoklu güçlendirici bonus
            }
            
            return Math.Max(0.0, Math.Min(1.0, score));
        }

        private string? ExtractKeywords(string text)
        {
            var lowerText = text.ToLower();
            
            // Mood'a özel anahtar kelimeler
            var moodKeywords = new Dictionary<string, string[]>
            {
                ["happy"] = new[] { "mutlu", "sevinç", "güzel", "harika", "mükemmel", "keyif", "neşe" },
                ["sad"] = new[] { "üzgün", "keder", "hüzün", "kederli", "mutsuz" },
                ["anxious"] = new[] { "korku", "endişe", "kaygı", "tedirgin", "gergin" },
                ["angry"] = new[] { "öfke", "kızgın", "sinir", "öfkeli", "kızgın" },
                ["calm"] = new[] { "sakin", "huzur", "dingin", "rahat", "sükunet" },
                ["energetic"] = new[] { "enerjik", "dinamik", "canlı", "hareketli", "aktif" }
            };
            
            var detectedMood = _mlService.PredictMood(text);
            var relevantKeywords = moodKeywords.GetValueOrDefault(detectedMood.ToLower(), new string[0]);
            
            // Metindeki kelimeleri al ve mood'a özel olanları öncelikle
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                           .Where(w => w.Length > 2) // 2 karakterden uzun
                           .Select(w => w.ToLower())
                           .Distinct()
                           .ToList();
            
            // Mood'a özel kelimeleri önce ekle
            var selectedKeywords = new List<string>();
            foreach (var keyword in relevantKeywords)
            {
                if (words.Contains(keyword) && selectedKeywords.Count < 3)
                {
                    selectedKeywords.Add(keyword);
                }
            }
            
            // Diğer önemli kelimeleri ekle
            var remainingWords = words.Where(w => !relevantKeywords.Contains(w) && w.Length > 3)
                                    .Take(3 - selectedKeywords.Count)
                                    .ToList();
            
            selectedKeywords.AddRange(remainingWords);
            
            return selectedKeywords.Count > 0 ? string.Join(", ", selectedKeywords) : null;
        }

        private async Task<List<Recommendation>> GetRecommendationsByMood(string? mood, int? userId)
        {
            var query = _context.Recommendations.AsQueryable();
            
            // Mood'a göre filtrele
            if (!string.IsNullOrEmpty(mood))
            {
                switch (mood.ToLower())
                {
                    case "happy":
                    case "mutlu":
                        query = query.Where(r => r.Genre == "Comedy" || r.Genre == "Adventure" || r.Genre == "Fantasy");
                        break;
                    case "sad":
                    case "üzgün":
                        query = query.Where(r => r.Genre == "Drama" || r.Genre == "Romance");
                        break;
                    case "anxious":
                    case "korku":
                        query = query.Where(r => r.Genre == "Thriller" || r.Genre == "Horror");
                        break;
                    case "angry":
                    case "öfke":
                        query = query.Where(r => r.Genre == "Action" || r.Genre == "Crime");
                        break;
                    case "calm":
                    case "sakin":
                        query = query.Where(r => r.Genre == "Drama" || r.Genre == "Documentary");
                        break;
                    case "energetic":
                    case "enerjik":
                        query = query.Where(r => r.Genre == "Action" || r.Genre == "Adventure");
                        break;
                }
            }
            
            // Kullanıcı tepkilerini dikkate al
            if (userId.HasValue)
            {
                var userReactions = await _context.UserReactions
                    .Where(ur => ur.UserId == userId && ur.ReactionType == "Like")
                    .Select(ur => ur.RecommendationId)
                    .ToListAsync();
                
                if (userReactions.Any())
                {
                    // Beğenilen türlere öncelik ver
                    var likedGenres = await _context.Recommendations
                        .Where(r => userReactions.Contains(r.Id))
                        .Select(r => r.Genre)
                        .Distinct()
                        .ToListAsync();
                    
                    if (likedGenres.Any())
                    {
                        query = query.OrderByDescending(r => likedGenres.Contains(r.Genre));
                    }
                }
            }
            
            // Rastgele sırala ve limit uygula
            return await query.OrderBy(r => Guid.NewGuid()).Take(3).ToListAsync();
        }


    }

    public class MoodAnalysisRequest
    {
        public string InputText { get; set; } = string.Empty;
        public int? UserId { get; set; }
    }
}
