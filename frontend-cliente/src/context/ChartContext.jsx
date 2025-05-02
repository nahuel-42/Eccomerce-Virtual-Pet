import { useState, useEffect, createContext } from "react";

// Creamos el nuevo contexto
export const ChartContext = createContext({ carrito: [], total: 0, cantidadTotal: 0 });

// Componente ChartProvider
export const ChartProvider = ({ children }) => {
    // Inicializamos el estado del carrito, intentando cargar desde localStorage
    const [carrito, setCarrito] = useState(() => {
        const savedCarrito = localStorage.getItem('carrito');
        return savedCarrito ? JSON.parse(savedCarrito) : [];
    });

    // Sincronizamos el carrito con localStorage cada vez que cambie
    useEffect(() => {
        localStorage.setItem('carrito', JSON.stringify(carrito));
    }, [carrito]);

    // Calculamos cantidadTotal y total dinámicamente
    const cantidadTotal = carrito.reduce((acc, prod) => acc + prod.cantidad, 0);
    const total = carrito.reduce((acc, prod) => acc + (prod.item.price * prod.cantidad), 0).toFixed(2);

    // Logs para depuración
    console.log(carrito);
    console.log('Cantidad de items: ' + cantidadTotal);
    console.log('Monto total de la compra: $ ' + total);

    // Función para agregar producto al carrito
    const agregarAlCarrito = (item, cantidad) => {
        const productoExistente = carrito.find(prod => prod.item.id === item.id);

        if (!productoExistente) {
            setCarrito(prev => [...prev, { item, cantidad }]);
        } else {
            const carritoActualizado = carrito.map(prod => {
                if (prod.item.id === item.id) {
                    return { ...prod, cantidad: prod.cantidad + cantidad };
                } else {
                    return prod;
                }
            });
            setCarrito(carritoActualizado);
        }
    };

    // Función para eliminar productos del carrito
    const eliminarProducto = (id) => {
        const carritoActualizado = carrito.filter(prod => prod.item.id !== id);
        setCarrito(carritoActualizado);
    };

    // Función para vaciar el carrito
    const vaciarCarrito = () => {
        setCarrito([]);
    };

    // Proveemos el contexto con los valores
    return (
        <ChartContext.Provider value={{ carrito, total, cantidadTotal, agregarAlCarrito, eliminarProducto, vaciarCarrito }}>
            {children}
        </ChartContext.Provider>
    );
};