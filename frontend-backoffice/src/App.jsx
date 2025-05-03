import React from 'react';
import { BrowserRouter, Route, Routes, useLocation } from 'react-router-dom';
import ItemDetailContainer from './components/ItemDetailContainer/ItemDetailContainer';
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
  const { isAuthenticated } = useAuth();

  const hideNav = ['/login', '/register'].includes(location.pathname);

  return (
    <>
      {!hideNav && <NavBar />}
      <Routes>
        <Route path="/item/:idItem" element={<ItemDetailContainer />} />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/register" element={<RegistrationForm />} />
        <Route
          path="/orders"
          element={
            // <PrivateRoute>
              <OrdersPage />
            // </PrivateRoute>
          }
        />
      </Routes>
      <Footer></Footer>
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
