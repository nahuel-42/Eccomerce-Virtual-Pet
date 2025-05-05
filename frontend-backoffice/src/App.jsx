import React from 'react';
import { BrowserRouter, Route, Routes, useLocation } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import LoginForm from './components/Auth/Login/Login';
import RegistrationForm from './components/Auth/Register/Register';
import NavBar from './components/NavBar/NavBar';
import PrivateRoute from './components/Auth/PrivateRoute';
import { ChartProvider } from './context/ChartContext';
import { AuthProvider, useAuth } from './context/AuthContext';
import Footer from './components/Footer/Footer';
import OrdersPage from './components/OrderPage/OrderPage';

function AppWrapper() {
  const location = useLocation();
  const hideNav = ['/admin/login'].includes(location.pathname);


  return (
    <>
      {!hideNav && <NavBar />}
      <Routes>
        <Route path="/admin/login" element={<LoginForm />} />
        <Route path="/admin/register" element={<PrivateRoute><RegistrationForm /></PrivateRoute>}/>
        <Route
          path="/admin"
          element={
            <PrivateRoute>
              <OrdersPage />
            </PrivateRoute>
          }
        />
      </Routes>
      <Footer />
      <ToastContainer position="top-right" autoClose={5000} />
    </>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <ChartProvider>
        <AuthProvider>
          <AppWrapper />
        </AuthProvider>
      </ChartProvider>
    </BrowserRouter>
  );
}
