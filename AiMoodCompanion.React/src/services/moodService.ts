import axios from 'axios';

const API_BASE_URL = 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add token to requests if available
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export interface MoodAnalysisRequest {
  inputText: string;
  userId?: number;
}

export interface Recommendation {
  id: number;
  title: string;
  description: string;
  type: string;
  genre?: string;
  year?: number;
  imageUrl?: string;
  externalId?: string;
}

export interface MoodAnalysisResponse {
  detectedMood: string;
  moodScore: number;
  keywords: string[];
  recommendations: Recommendation[];
}

export interface UserReactionRequest {
  recommendationId: number;
  reactionType: string; // 'Like', 'Dislike', 'WatchLater'
}

export const moodService = {
  async analyzeMood(request: MoodAnalysisRequest): Promise<MoodAnalysisResponse> {
    const response = await api.post<MoodAnalysisResponse>('/mood/analyze', request);
    return response.data;
  },

  async analyzeMoodAnonymous(request: MoodAnalysisRequest): Promise<MoodAnalysisResponse> {
    const response = await api.post<MoodAnalysisResponse>('/mood/analyze-anonymous', request);
    return response.data;
  },

  async addReaction(request: UserReactionRequest): Promise<void> {
    await api.post('/user/reaction', request);
  },

  async getLikedItems(): Promise<Recommendation[]> {
    const response = await api.get<Recommendation[]>('/user/liked');
    return response.data;
  },

  async getWatchlist(): Promise<Recommendation[]> {
    const response = await api.get<Recommendation[]>('/user/watchlist');
    return response.data;
  },

  async searchRecommendations(query: string): Promise<Recommendation[]> {
    const response = await api.get<Recommendation[]>(`/mood/search?q=${encodeURIComponent(query)}`);
    return response.data;
  },
};
