import React, { useState } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import AuthService from '../../../services/auth.service';
import { toast } from 'react-toastify';
import '../Auth.css';

export function RegistrationForm(e) {
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
        await AuthService.register({ name, email, password });
        toast.success('Usuario registrado correctamente');
        setName('');
        setEmail('');
        setPassword('');
        setConfirm('');
      } catch (err) {
        setError(err.message || 'Error al registrarse');
      }
    };
  
    return (
      <section className='register-admin-page'>
        <div className="register-admin-container">
          <div className="auth-card">
            <h2 className="auth-title">Crea una cuenta</h2>
            <p className="auth-subtitle">Registrar nuevo administrador</p>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-4 form-style">
                <Form.Label>Nombre Completo</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Nombre"
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
      
              <Button type="submit" className="submit-btn w-100 my-3">
                Registrar
              </Button>
            </Form>
          </div>
        </div>
      </section>
    );
}

export default RegistrationForm;
