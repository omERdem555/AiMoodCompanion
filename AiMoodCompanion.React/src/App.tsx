import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import Navbar from './components/Navbar';
import SignUp from './pages/SignUp';
import Login from './pages/Login';
import MoodSearch from './pages/MoodSearch';
import MainMenu from './pages/MainMenu';
import AnonymousSearch from './pages/AnonymousSearch';
import PrivateRoute from './components/PrivateRoute';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="min-h-screen bg-gray-50">
          <Navbar />
          <main className="container mx-auto px-4 py-8">
            <Routes>
              <Route path="/signup" element={<SignUp />} />
              <Route path="/login" element={<Login />} />
              <Route path="/mood-search" element={
                <PrivateRoute>
                  <MoodSearch />
                </PrivateRoute>
              } />
              <Route path="/main-menu" element={
                <PrivateRoute>
                  <MainMenu />
                </PrivateRoute>
              } />
              <Route path="/anonymous" element={<AnonymousSearch />} />
              <Route path="/" element={<Navigate to="/mood-search" replace />} />
            </Routes>
          </main>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
