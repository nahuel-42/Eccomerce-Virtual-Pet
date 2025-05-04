// src/services/AuthService.js
import API_BASE_URL from './config';

const API_URL = `${API_BASE_URL}/user`;

const AuthService = {
  login: async ({ email, password }) => {
    const response = await fetch(`${API_URL}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al iniciar sesión');
    }

    const data = await response.json();
    return data;
  },

  register: async ({ name, email, password }) => {
    const currentAdmin = JSON.parse(AuthService.getUserId());
    const response = await fetch(`${API_URL}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name,
        email,
        password,
        adminId: currentAdmin,
      }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al registrarse');
    }

    const data = await response.json();
    return data;
  },

  saveToken: (token) => {
    localStorage.setItem('token', token);
  },

  getToken: () => {
    return localStorage.getItem('token');
  },

  saveUserId: (userId) => {
    localStorage.setItem('userId', userId);
  },

  getUserId: () => {
    return localStorage.getItem('userId');
  },

  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
  },
};

export default AuthService;
