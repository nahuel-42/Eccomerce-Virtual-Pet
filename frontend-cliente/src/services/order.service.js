import API_BASE_URL from './config';
const API_URL = `${API_BASE_URL}/orders`;

const OrderService = {
    async createOrder(orden) {
        try {
            // Obtenemos el token desde localStorage
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error("No se encontró un token de autenticación. Por favor, inicia sesión.");
            }

            const response = await fetch(`${API_URL}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(orden)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(`Error en la API: ${response.status} ${errorData.message || response.statusText}`);
            }

            const data = await response.json();
            return data.orderId; 
        } catch (err) {
            throw new Error(err.message || "No se pudo completar la orden. Intenta nuevamente.");
        }
    }
};

export default OrderService;