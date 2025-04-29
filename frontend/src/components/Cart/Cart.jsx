import CartItem from "../CartItem/CartItem";
import { Link } from "react-router-dom";
import { ChartContext } from "../../context/ChartContext";
import { useContext } from "react";
import './Cart.css';
import Button from 'react-bootstrap/Button';


const Cart = () => {
    const { carrito, vaciarCarrito, total, cantidadTotal } = useContext(ChartContext);

    if (cantidadTotal === 0) {
        return (
            <div className="div-vacio">
                <h2>No hay productos en el carrito</h2>
                <Link to="/"> <Button variant="secondary">Ver productos</Button> </Link>
            </div>
        )
    }
    return (
        <div className="div-container">
            <div className="div-cart">
                {
                    carrito.map(prod => <div><CartItem key={prod.id} {...prod} /><span className='chart-span'> </span></div>)
                }

                <h3> Total:$ {total}  </h3>

                <div className="btns">
                    <Link to="/" style={{ textDecoration: 'none' }}> <Button className='btn-prods' variant="secondary">Ver más productos</Button> </Link>
                    <Link to="/checkout" style={{ textDecoration: 'none' }}> <Button className='btn-fin' variant="dark">Finalizar compra</Button> </Link>
                </div>
            </div>
            <Button className="btn-vacio" size="sm" onClick={() => vaciarCarrito()} variant="danger" style={{ width: '10%' }}>Vaciar Carrito</Button>
        </div>

    )
}

export default Cart