// src/services/product.service.js
import API_BASE_URL from './config';

export async function fetchProducts() {
  try {
    const response = await fetch(`${API_BASE_URL}/products`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al cargar los productos');
    }

    const data = await response.json();
    return data;
  } catch (error) {
    throw new Error(error.message || 'Error de red al cargar los productos');
  }
}

export async function fetchProduct(id) {
  try {
    const response = await fetch(`${API_BASE_URL}/products/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al cargar el producto');
    }

    const data = await response.json();
    return data;
  } catch (error) {
    throw new Error(error.message || 'Error de red al cargar el producto');
  }
}

