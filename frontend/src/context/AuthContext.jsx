// src/context/AuthContext.js
import React, { createContext, useContext, useState, useEffect } from 'react';
import AuthService from '../services/auth.service';

const AuthContext = createContext();

// Proveedor de contexto para que toda la aplicación pueda acceder al estado de autenticación
export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);

  // Verificamos si el usuario tiene un token en el localStorage al cargar la aplicación
  useEffect(() => {
    const token = AuthService.getToken();
    if (token) {
      setIsAuthenticated(true);
      // Aquí podrías hacer una llamada al backend para obtener los datos del usuario
      setUser({ id: 1, name: 'Usuario', role: 'admin' }); // Esto es solo un ejemplo
    }
  }, []);

  // Función para hacer login
  const login = (token) => {
    AuthService.saveToken(token);
    setIsAuthenticated(true);
  };

  // Función para hacer logout
  const logout = () => {
    AuthService.logout();
    setIsAuthenticated(false);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// Hook para acceder al contexto
export const useAuth = () => useContext(AuthContext);
