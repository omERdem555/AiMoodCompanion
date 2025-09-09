import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { moodService, Recommendation } from '../services/moodService';
import { Link } from 'react-router-dom';

const MoodSearch: React.FC = () => {
  const [inputText, setInputText] = useState('');
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [analysisResult, setAnalysisResult] = useState<any>(null);
  const [error, setError] = useState('');
  const { user } = useAuth();

  const handleAnalyze = async () => {
    if (!inputText.trim()) {
      setError('Please enter some text to analyze');
      return;
    }

    setIsAnalyzing(true);
    setError('');
    setAnalysisResult(null);

    try {
      const result = await moodService.analyzeMood({
        inputText: inputText.trim(),
        userId: user?.id
      });
      setAnalysisResult(result);
    } catch (err: any) {
      setError(err.response?.data || 'Analysis failed');
    } finally {
      setIsAnalyzing(false);
    }
  };

  const handleReaction = async (recommendationId: number, reactionType: string) => {
    try {
      await moodService.addReaction({ recommendationId, reactionType });
      // Show success feedback
      alert(`Added to ${reactionType === 'Like' ? 'likes' : 'watchlist'}!`);
    } catch (err: any) {
      alert('Failed to save reaction');
    }
  };

  const getMoodEmoji = (mood: string) => {
    switch (mood.toLowerCase()) {
      case 'happy': return '😊';
      case 'sad': return '😢';
      case 'excited': return '🎉';
      case 'relaxed': return '😌';
      default: return '😐';
    }
  };

  return (
    <div className="max-w-6xl mx-auto px-4">
      <div className="text-center mb-8">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">
          How are you feeling today?
        </h1>
        <p className="text-xl text-gray-600">
          Tell us about your mood and get personalized recommendations
        </p>
      </div>

      <div className="bg-white rounded-lg shadow-lg p-6 mb-8">
        <div className="mb-4">
          <label htmlFor="moodInput" className="block text-sm font-medium text-gray-700 mb-2">
            Describe your current mood or how you're feeling:
          </label>
          <textarea
            id="moodInput"
            rows={4}
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
            placeholder="e.g., I'm feeling really happy today because I got good news, or I'm a bit stressed about work..."
            value={inputText}
            onChange={(e) => setInputText(e.target.value)}
          />
        </div>

        <div className="flex justify-center">
          <button
            onClick={handleAnalyze}
            disabled={isAnalyzing || !inputText.trim()}
            className="bg-primary-600 hover:bg-primary-700 disabled:bg-gray-400 text-white px-8 py-3 rounded-lg font-medium text-lg transition-colors"
          >
            {isAnalyzing ? 'Analyzing...' : 'Analyze My Mood'}
          </button>
        </div>

        {error && (
          <div className="mt-4 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
            {error}
          </div>
        )}
      </div>

      {analysisResult && (
        <div className="space-y-8">
          {/* Mood Analysis Result */}
          <div className="bg-white rounded-lg shadow-lg p-6">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">
              Mood Analysis Results
            </h2>
            <div className="grid md:grid-cols-3 gap-6">
              <div className="text-center">
                <div className="text-6xl mb-2">
                  {getMoodEmoji(analysisResult.detectedMood)}
                </div>
                <h3 className="text-xl font-semibold text-gray-800">
                  {analysisResult.detectedMood}
                </h3>
                <p className="text-gray-600">Detected Mood</p>
              </div>
              <div className="text-center">
                <div className="text-4xl font-bold text-primary-600 mb-2">
                  {(analysisResult.moodScore * 100).toFixed(0)}%
                </div>
                <h3 className="text-xl font-semibold text-gray-800">Mood Score</h3>
                <p className="text-gray-600">Confidence Level</p>
              </div>
              <div className="text-center">
                <div className="text-4xl font-bold text-secondary-600 mb-2">
                  {analysisResult.keywords.length}
                </div>
                <h3 className="text-xl font-semibold text-gray-800">Keywords</h3>
                <p className="text-gray-600">Identified</p>
              </div>
            </div>

            {analysisResult.keywords.length > 0 && (
              <div className="mt-6">
                <h4 className="text-lg font-semibold text-gray-800 mb-3">Keywords Found:</h4>
                <div className="flex flex-wrap gap-2">
                  {analysisResult.keywords.map((keyword: string, index: number) => (
                    <span
                      key={index}
                      className="bg-primary-100 text-primary-800 px-3 py-1 rounded-full text-sm font-medium"
                    >
                      {keyword}
                    </span>
                  ))}
                </div>
              </div>
            )}
          </div>

          {/* Recommendations */}
          <div className="bg-white rounded-lg shadow-lg p-6">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-2xl font-bold text-gray-900">
                Personalized Recommendations
              </h2>
              <Link
                to="/main-menu"
                className="bg-secondary-600 hover:bg-secondary-700 text-white px-4 py-2 rounded-lg font-medium"
              >
                View All My Items
              </Link>
            </div>

            <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
              {analysisResult.recommendations.map((recommendation: Recommendation) => (
                <div key={recommendation.id} className="bg-gray-50 rounded-lg p-4">
                  {recommendation.imageUrl && (
                    <img
                      src={recommendation.imageUrl}
                      alt={recommendation.title}
                      className="w-full h-48 object-cover rounded-lg mb-4"
                    />
                  )}
                  <div className="mb-4">
                    <div className="flex items-center justify-between mb-2">
                      <span className="bg-primary-100 text-primary-800 px-2 py-1 rounded text-xs font-medium">
                        {recommendation.type}
                      </span>
                      {recommendation.year && (
                        <span className="text-gray-500 text-sm">{recommendation.year}</span>
                      )}
                    </div>
                    <h3 className="text-lg font-semibold text-gray-900 mb-2">
                      {recommendation.title}
                    </h3>
                    <p className="text-gray-600 text-sm mb-2">
                      {recommendation.description}
                    </p>
                    {recommendation.genre && (
                      <span className="text-gray-500 text-xs">{recommendation.genre}</span>
                    )}
                  </div>
                  
                  <div className="flex gap-2">
                    <button
                      onClick={() => handleReaction(recommendation.id, 'Like')}
                      className="flex-1 bg-green-600 hover:bg-green-700 text-white px-3 py-2 rounded text-sm font-medium"
                    >
                      👍 Like
                    </button>
                    <button
                      onClick={() => handleReaction(recommendation.id, 'WatchLater')}
                      className="flex-1 bg-blue-600 hover:bg-blue-700 text-white px-3 py-2 rounded text-sm font-medium"
                    >
                      📺 Watch Later
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default MoodSearch;
