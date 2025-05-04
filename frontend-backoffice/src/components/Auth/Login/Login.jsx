import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../../context/AuthContext';
import AuthService from '../../../services/auth.service';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import '../Auth.css';

export function LoginForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);

  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      const response = await AuthService.login({ email, password });

      if (response.user?.role?.id !== 1) {
        setError('No tenés permisos para acceder a esta aplicación.');
        return;
      }

      login(response.token, response.user.id);
      navigate('/');
    } catch (err) {
      setError(err.message || 'Error al iniciar sesión');
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2 className="auth-title">¡Bienvenido de nuevo!</h2>
        <p className="auth-subtitle">Ingresa tus datos para continuar</p>
        <Form onSubmit={handleSubmit}>
          <Form.Group className="mb-4 form-style">
            <Form.Label>Correo Electrónico</Form.Label>
            <Form.Control
              type="email"
              placeholder="usuario@ejemplo.com"
              value={email}
              onChange={e => setEmail(e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group className="mb-4 form-style">
            <Form.Label>Contraseña</Form.Label>
            <Form.Control
              type="password"
              placeholder="••••••••"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
            />
          </Form.Group>

          {error && <div className="error-message">{error}</div>}

          <Button type="submit" className="submit-btn w-100 my-3">
            Entrar
          </Button>
        </Form>
      </div>
    </div>
  );
}

export default LoginForm;
