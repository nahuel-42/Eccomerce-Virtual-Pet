import { useState, useContext, useEffect } from "react";
import { ChartContext } from "../../context/ChartContext";
import { Link } from "react-router-dom";
import CartItem from "../CartItem/CartItem";
import OrderService from "../../services/order.service";
import './Checkout.css';

const Checkout = () => {
    const { carrito, vaciarCarrito, total } = useContext(ChartContext);
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [orderId, setOrderId] = useState("");
    const [error, setError] = useState("");

    const manejadorSubmit = async (event) => {
        event.preventDefault();
        // Verificamos que todos los campos se completen
        if (!address || !phone) {
            setError("¡Por favor completa todos los campos!");
            return;
        }

        // Creamos un objeto con todos los datos de la orden
        const orden = {
            products: carrito.map(producto => ({
                productId: producto.item.id,
                quantity: producto.cantidad
            })),
            address,
            phone
        };

        console.log('ORDEN: ' + JSON.stringify(orden));

        try {
            // Usamos el servicio para crear la orden
            const newOrderId = await OrderService.createOrder(orden);

            // Actualizamos el estado y vaciamos el carrito
            setOrderId(newOrderId);
            vaciarCarrito();
        } catch (err) {
            console.error("Error al enviar la orden:", err);
            setError(err.message);
        }
    };

    return (
        <div className="container mb-5 vh-100">
            {orderId ? (
                <div className="row w-100 mt-3 justify-content-center">
                    <div className="col-8 text-center">
                        <div className="mt-4 p-3 fs-1 fw-medium">
                            <p className='me-3'>¡Gracias por tu compra!</p>
                            <div className="d-flex flex-row justify-content-center p-2 align-bottom">
                                <p className='me-3'>Tu número de orden es:</p>
                                <p className='px-2 fs-1 fw-semibold text-white bg-success rounded bg-opacity-75'>#{orderId}</p>
                            </div>
                        </div>
                        <Link to="/"> <button className="btn btn-secondary m-2">Ver pedidos</button> </Link>
                    </div>
                </div>
            ) : (
                <div className="row w-100 mt-3">
                    <div className="col-8">
                        <form onSubmit={manejadorSubmit} className="d-flex flex-column justify-content-start align-items-start w-100">
                            <p className="fs-2 fw-semibold">Datos de contacto </p>
                            <div className="w-75">
                                <label htmlFor="address">Dirección</label>
                                <input 
                                    className="rounded-3 border-1"
                                    type="text" 
                                    value={address} 
                                    id="address" 
                                    onChange={(e) => setAddress(e.target.value)} 
                                />
                            </div>

                            <div className="w-75 mt-4">
                                <label htmlFor="phone">Teléfono</label>
                                <input 
                                    className="rounded-3 border-1"
                                    type="text" 
                                    value={phone} 
                                    id="phone" 
                                    onChange={(e) => setPhone(e.target.value)} 
                                />
                            </div>

                            {
                                error && <p style={{ color: "red" }}> {error} </p>
                            }

                            <div className="w-75 justify-content-end align-items-end d-flex flex-row mt-4">
                                <button 
                                    type="submit" 
                                    disabled={carrito.length === 0} 
                                    className="btn btn-dark mt-2"
                                >
                                    Finalizar Orden
                                </button>
                            </div>
                        </form>
                    </div>
                    <div className="col-4">
                        <div className="position-relative bg-white w-100 h-200 justify-content-center bg-opacity-50 rounded-3 shadow p-3">
                            <p className="fs-2 fw-semibold p-2">Resumen de compra</p>
                            <div className="d-flex flex-column w-100 overflow-y-auto" style={{ maxHeight: '200px' }}> 
                            {
                                carrito.map(prod => (
                                    <div key={prod.item.id}>
                                        <CartItem {...prod} />
                                    </div>
                                ))
                            }
                            </div>
                            <div className="w-100">
                                <hr />
                                <div className="d-flex flex-row justify-content-between p-2 align-bottom">
                                    <p className='fs-1 fw-semibold me-3'>Total: </p>
                                    <p className='fs-1 fw-semibold text-success'>$ {total} </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Checkout;