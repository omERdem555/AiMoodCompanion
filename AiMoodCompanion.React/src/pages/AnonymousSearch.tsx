import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { moodService, Recommendation } from '../services/moodService';

const AnonymousSearch: React.FC = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState<Recommendation[]>([]);
  const [isSearching, setIsSearching] = useState(false);
  const [error, setError] = useState('');

  const handleSearch = async () => {
    if (!searchQuery.trim()) {
      setError('Please enter a search term');
      return;
    }

    setIsSearching(true);
    setError('');

    try {
      // For anonymous search, we'll use a simple text-based search
      // In a real app, this would call an API endpoint that doesn't require authentication
      const results = await moodService.searchRecommendations(searchQuery.trim());
      setSearchResults(results);
    } catch (err: any) {
      setError('Search failed. Please try again.');
      console.error('Search error:', err);
    } finally {
      setIsSearching(false);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <div className="max-w-6xl mx-auto px-4">
      <div className="text-center mb-8">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">
          Anonymous Search
        </h1>
        <p className="text-xl text-gray-600">
          Search for recommendations without creating an account
        </p>
        <div className="mt-4">
          <Link
            to="/signup"
            className="text-primary-600 hover:text-primary-700 font-medium"
          >
            Or create an account for personalized recommendations →
          </Link>
        </div>
      </div>

      <div className="bg-white rounded-lg shadow-lg p-6 mb-8">
        <div className="mb-4">
          <label htmlFor="searchInput" className="block text-sm font-medium text-gray-700 mb-2">
            What are you looking for?
          </label>
          <div className="flex gap-2">
            <input
              id="searchInput"
              type="text"
              className="flex-1 px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary-500 focus:border-transparent"
              placeholder="e.g., happy movies, relaxing music, exciting books..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              onKeyPress={handleKeyPress}
            />
            <button
              onClick={handleSearch}
              disabled={isSearching || !searchQuery.trim()}
              className="bg-primary-600 hover:bg-primary-700 disabled:bg-gray-400 text-white px-6 py-2 rounded-md font-medium transition-colors"
            >
              {isSearching ? 'Searching...' : 'Search'}
            </button>
          </div>
        </div>

        {error && (
          <div className="mt-4 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
            {error}
          </div>
        )}
      </div>

      {searchResults.length > 0 && (
        <div className="bg-white rounded-lg shadow-lg p-6">
          <h2 className="text-2xl font-bold text-gray-900 mb-6">
            Search Results ({searchResults.length})
          </h2>

          <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
            {searchResults.map((recommendation: Recommendation) => (
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
                
                <div className="text-center">
                  <Link
                    to="/signup"
                    className="inline-block bg-primary-600 hover:bg-primary-700 text-white px-4 py-2 rounded text-sm font-medium"
                  >
                    Sign Up to Save
                  </Link>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {searchResults.length === 0 && !isSearching && !error && searchQuery && (
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">🔍</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-2">
            No results found
          </h3>
          <p className="text-gray-600 mb-4">
            Try different keywords or check your spelling
          </p>
        </div>
      )}
    </div>
  );
};

export default AnonymousSearch;
