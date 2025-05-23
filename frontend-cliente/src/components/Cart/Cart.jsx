import CartItem from "../CartItem/CartItem";
import { Link } from "react-router-dom";
import { ChartContext } from "../../context/ChartContext";
import { useContext } from "react";
import './Cart.css';
import { useAuth } from '../../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Cart = () => {
    const { carrito, vaciarCarrito, total, cantidadTotal } = useContext(ChartContext);
    const { isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleCheckout = () => {
        if (!isAuthenticated) {
            navigate('/login');
        } else {
            navigate('/checkout');
        }
    };

    if (cantidadTotal === 0) {
        return (
            <div className="container d-flex flex-column justify-content-center align-items-center mt-5">
                <h2>No hay productos en el carrito</h2>
                <Link to="/"> <button className="btn btn-secondary m-2">Ver productos</button> </Link>
            </div>
        )
    }
    
    return (
        <div className="container mb-5 vh-100">
            <div className="row w-100 mt-3">
                <div className="col-8 justify-content-between">
                    <p className="fs-2 fw-semibold">Mi carrito </p>
                    <div className="d-flex flex-row justify-content-between">
                        <p className="fs-5 fw-semibold">Producto</p>
                        <p className="fs-5 fw-semibold me-3">Subtotal</p>
                    </div>
                    <div className="d-flex flex-column card w-100 p-3 "> 
                    {
                        carrito.map(prod => (
                            <div key={prod.item.id}>
                                <CartItem {...prod} />
                            </div>
                        ))
                    }
                    </div>
                    <button className="btn btn-secondary mt-2" size="sm" onClick={() => vaciarCarrito()}>Vaciar Carrito</button>
                </div>
                <div className="col-4">
                    <div className="bg-white w-100 justify-content-center align-items-center bg-opacity-50 rounded-3 shadow p-3">
                        <p className="fs-2 fw-semibold">Pedido actual</p>
                        <div className="w-100">
                            <div className="d-flex flex-row justify-content-between p-2">
                                <p className='fs-1 fw-semibold me-3'>Total: </p>
                                <p className='fs-1 fw-semibold text-success'>$ {total}  </p>
                            </div>
                            <div className="d-flex flex-column">
                                <button className='btn btn-dark w-100 mt-2' onClick={handleCheckout}>
                                    Finalizar compra
                                </button>
                                <Link to="/" style={{ textDecoration: 'none' }}> <button className='btn btn-secondary w-100 mt-2'>Ver más productos</button> </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    )
}

export default Cart