import { useState, useEffect } from "react";
import OrderService from "../../services/order.service";
import { Collapse } from "react-bootstrap";
import { OrderStatus, ORDER_STATUS_OPTIONS, ORDER_STATUS_COLORS } from "./constants/orderStatus"; 

const Orders = () => {
    const [orders, setOrders] = useState([]);
    const [error, setError] = useState("");
    const [openSections, setOpenSections] = useState({});

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const ordersData = await OrderService.getOrders();
                setOrders(ordersData);
            } catch (err) {
                console.error("Error al obtener los pedidos:", err);
                setError(err.message);
            }
        };

        fetchOrders();
    }, []);

    const toggleSection = (orderId) => {
        setOpenSections(prev => ({
            ...prev,
            [orderId]: !prev[orderId]
        }));
    };

    const getStatusLabel = (status) => {
        const option = ORDER_STATUS_OPTIONS.find(opt => opt.value === status);
        return option ? option.label : "Desconocido";
    };

    return (
        <div className="container my-5">
            <p className="fs-2 fw-semibold">Mis pedidos</p>
            {error && <p className="text-danger">{error}</p>}
            {orders.length === 0 && !error ? (
                <p>No tienes pedidos aún.</p>
            ) : (
                <div className="row w-100">
                    {orders.map(order => (
                        <div key={order.id} className="col-12 mb-4">
                            <div className="card shadow-sm w-100 position-relative">
                                {/* Badge de estado en la esquina superior derecha */}
                                <span
                                    className={`badge position-absolute top-0 end-0 m-3 text-bg-secondary`}
                                >
                                    {getStatusLabel(order.status)}
                                </span>
                                <div className="card-body">
                                    <h5 className="card-title">Pedido #{order.id}</h5>
                                    <p className="card-text">
                                        <strong>Dirección:</strong> {order.address}<br />
                                        <strong>Teléfono:</strong> {order.phone}<br />
                                        <strong>Fecha:</strong> {new Date(order.createdDate).toLocaleDateString()}
                                    </p>
                                    {/* Toggle header para productos */}
                                    <h6
                                        onClick={() => toggleSection(order.id)}
                                        className="toggle-header"
                                        style={{ cursor: "pointer", userSelect: "none" }}
                                    >
                                        Productos {openSections[order.id] ? "▼" : "▶"}
                                    </h6>
                                    <Collapse in={openSections[order.id]}>
                                        <div>
                                            <ul className="list-group list-group-flush py-2">
                                                {order.products.map(product => (
                                                   <div key={order.id+product.id} className='fw-medium fs-6 justify-content-between d-flex flex-row px-2 py-1'>
                                                        <p className='me-3'>{product.name}</p>
                                                
                                                        <div className="d-flex flex-row">
                                                            <p className='text-secondary'>{product.quantity} x</p>
                                                            <p className='mx-2 fw-semibold'>  $ {product.unitPrice} </p>
                                                        </div>
                                                    </div>
                                                ))}
                                            </ul>
                                        </div>
                                    </Collapse>
                                    <div className="d-flex flex-row justify-content-end">
                                        <p className='fs-2 fw-semibold me-3'>Total: </p>
                                        <p className='fs-2 fw-semibold text-success'>$ {order.totalPrice}  </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default Orders;