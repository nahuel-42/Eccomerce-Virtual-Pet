import React from 'react';
import { BrowserRouter, Route, Routes, useLocation } from 'react-router-dom';
import ItemListContainer from './components/ItemListContainer/ItemListContainer';
import ItemDetailContainer from './components/ItemDetailContainer/ItemDetailContainer';
import Cart from './components/Cart/Cart';
import Checkout from './components/Checkout/Checkout';
import LoginForm from './components/Auth/Login/Login';
import RegistrationForm from './components/Auth/Register/Register';
import NavBar from './components/NavBar/NavBar';
import PrivateRoute from './components/Auth/PrivateRoute';
import { ChartProvider } from './context/ChartContext';
import { AuthProvider, useAuth } from './context/AuthContext';
import Footer from './components/Footer/Footer';

function AppWrapper() {
  const location = useLocation();
  const { isAuthenticated } = useAuth();

  const hideNav = ['/login', '/register'].includes(location.pathname);

  return (
    <>
      {!hideNav && <NavBar />}
      <Routes>
        <Route path="/" element={<ItemListContainer />} />
        <Route path="/category/:idCategoria" element={<ItemListContainer />} />
        <Route path="/item/:idItem" element={<ItemDetailContainer />} />
        <Route path="/cart" element={
          <PrivateRoute>
            <Cart />
          </PrivateRoute>
        } />
        <Route path="/checkout" element={
          <PrivateRoute>
            <Checkout />
          </PrivateRoute>
        } />
        <Route path="/login" element={<LoginForm />} />
        <Route path="/register" element={<RegistrationForm />} />
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
