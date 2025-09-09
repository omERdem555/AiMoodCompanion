import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const MainMenu: React.FC = () => {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <div className="max-w-6xl mx-auto px-4">
      <div className="text-center mb-8">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">
          Welcome, {user?.name || 'User'}!
        </h1>
        <p className="text-xl text-gray-600">
          What would you like to do today?
        </p>
      </div>

      <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
        {/* Mood Analysis */}
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">😊</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-3">
            Analyze My Mood
          </h3>
          <p className="text-gray-600 mb-4">
            Get personalized recommendations based on your current mood
          </p>
          <Link
            to="/mood-search"
            className="inline-block bg-primary-600 hover:bg-primary-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            Start Analysis
          </Link>
        </div>

        {/* My Recommendations */}
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">📚</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-3">
            My Recommendations
          </h3>
          <p className="text-gray-600 mb-4">
            View your saved likes and watchlist items
          </p>
          <Link
            to="/recommendations"
            className="inline-block bg-secondary-600 hover:bg-secondary-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            View My Items
          </Link>
        </div>

        {/* Mood History */}
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">📊</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-3">
            Mood History
          </h3>
          <p className="text-gray-600 mb-4">
            Track your mood patterns over time
          </p>
          <Link
            to="/mood-history"
            className="inline-block bg-green-600 hover:bg-green-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            View History
          </Link>
        </div>

        {/* Anonymous Search */}
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">🔍</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-3">
            Anonymous Search
          </h3>
          <p className="text-gray-600 mb-4">
            Search for recommendations without logging in
          </p>
          <Link
            to="/anonymous"
            className="inline-block bg-purple-600 hover:bg-purple-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            Try Anonymous
          </Link>
        </div>

        {/* Profile Settings */}
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">⚙️</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-3">
            Profile Settings
          </h3>
          <p className="text-gray-600 mb-4">
            Manage your account and preferences
          </p>
          <Link
            to="/profile"
            className="inline-block bg-gray-600 hover:bg-gray-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            Settings
          </Link>
        </div>

        {/* Logout */}
        <div className="bg-white rounded-lg shadow-lg p-6 text-center">
          <div className="text-6xl mb-4">🚪</div>
          <h3 className="text-xl font-semibold text-gray-800 mb-3">
            Logout
          </h3>
          <p className="text-gray-600 mb-4">
            Sign out of your account
          </p>
          <button
            onClick={handleLogout}
            className="inline-block bg-red-600 hover:bg-red-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            Logout
          </button>
        </div>
      </div>
    </div>
  );
};

export default MainMenu;
