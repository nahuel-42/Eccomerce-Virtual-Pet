import React, { useState } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import '../Auth.css';

/**
 * RegistrationForm.jsx
 * Formulario de registro con el mismo estilo moderno.
 */
export function RegistrationForm({ onRegister }) {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirm, setConfirm] = useState('');
    const [error, setError] = useState(null);
  
    const handleSubmit = async (e) => {
      e.preventDefault();
      if (password !== confirm) {
        setError('Las contraseñas no coinciden');
        return;
      }
      setError(null);
      try {
        await onRegister({ name, email, password });
      } catch (err) {
        setError(err.message || 'Error al registrarse');
      }
    };
  
    return (
      <div className="auth-container">
        <div className="auth-card">
          <h2 className="auth-title">Crea tu cuenta</h2>
          <p className="auth-subtitle">Regístrate para comenzar</p>
          <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-4 form-style">
              <Form.Label>Nombre Completo</Form.Label>
              <Form.Control
                type="text"
                placeholder="Tu nombre"
                value={name}
                onChange={e => setName(e.target.value)}
                required
              />
            </Form.Group>
    
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
    
            <Form.Group className="mb-4 form-style">
              <Form.Label>Confirmar Contraseña</Form.Label>
              <Form.Control
                type="password"
                placeholder="••••••••"
                value={confirm}
                onChange={e => setConfirm(e.target.value)}
                required
              />
            </Form.Group>
    
            {error && <div className="error-message">{error}</div>}
    
            <Button type="submit" className="submit-btn w-100 mb-3">
              Registrarse
            </Button>
          </Form>
          <div className="auth-footer">
            <a href="/login" className="auth-link">¿Ya tienes cuenta? Iniciar Sesión</a>
          </div>
        </div>
      </div>
    );
}

export default RegistrationForm;
