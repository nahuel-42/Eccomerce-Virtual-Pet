//1) Voy a importar el createContext que me permite crear un contexto que almacenará toda la lógica de mi carro de compras y el hook useState
import { useState, createContext } from "react";

//2) Creamos el nuevo contexto y le doy como valor inicial un objeto que tiene las siguientes propiedades:
export const ChartContext = createContext({ carrito: [], total: 0, cantidadTotal: 0 })

//3) Creamos el componente llamado ChartProvider (Proveedor de contextos)

export const ChartProvider = ({children}) => {
    //Usamos useState para generar algunos estados para almacenar el carrito, el total y cantidadTotal
    const [carrito, setCarrito] = useState([]);
    const [total, setTotal] = useState(0);
    const [cantidadTotal, setCantidadTotal] = useState(0);


    //Metemos unos console.log de forma momentánea para ver que se esté actualizando todo correctamente
    console.log(carrito);
    console.log('Cantidad de items: ' + cantidadTotal);
    console.log('Monto total de la compra: $ ' + total);

    //Funcion agregar Producto al carrito
    const agregarAlCarrito = ( item, cantidad ) => {
        console.log(item);
        const productoExistente = carrito.find(prod => prod.item.id === item.id);

        if (!productoExistente) {
            //un nuevo estado con el array anterior pero ahora se le suma un objeto.
            //La sintaxis prev => [...prev, {item, cantidad}] se usa para crear un nuevo array a partir del estado anterior del carrito (prev) y agregar un nuevo objeto.
            setCarrito(prev => [...prev, { item, cantidad }]);
            setCantidadTotal(prev => prev + cantidad);
            setTotal(prev => prev + (item.precio * cantidad));
        } else {
            const carritoActualizado = carrito.map(prod => {
                if (prod.item.id === item.id) {
                    return { ...prod, cantidad: prod.cantidad + cantidad }
                } else {
                    return prod;
                }
            });
            setCarrito(carritoActualizado);
            setCantidadTotal(prev => prev + cantidad);
            setTotal(prev => prev + (item.precio * cantidad));
        }
    }


    //función para eliminar productos del carrito
    const eliminarProducto = (id) => {
        //me guardo una referencia de producto que vamos a borrar 
        const productoEliminado = carrito.find(prod => prod.item.id === id);
        //ahora lo elimino del array
        const carritoActualizado = carrito.filter(prod => prod.item.id !== id);

        setCarrito(carritoActualizado);
        setCantidadTotal(prev => prev - productoEliminado.cantidad);
        setTotal(prev => prev - (productoEliminado.precio * cantidad));
    }

    //función vaciar el carrito de compras 

    const vaciarCarrito = () => {
        setCarrito([]);
        setTotal(0);
        setCantidadTotal(0);
    }

    //usamos el value para enviar el valor del carrito...
    //van dos llaves para pasar un objeto
    return (
        <ChartContext.Provider value={{carrito, total, cantidadTotal, agregarAlCarrito, eliminarProducto, vaciarCarrito}}> {children} </ChartContext.Provider>
    )

    //le tenemos que agregar el children, que es una propiedad especial que utilizamos para representar a todos esos componentes que puedan necesitar el carrito y sus funciones
}

