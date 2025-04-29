import { useState, useEffect } from "react";
import Button from 'react-bootstrap/Button';
import './Counter.css';

const Counter = ({ inicial, stock, funcionAgregar }) => {

    const [contador, setContador] = useState(1);

    const incrementar = () => {
        if (contador < stock) {
            setContador(contador + 1);
        }
    }

    const decrementar = () => {
        if (contador > inicial) {
            setContador(contador - 1);
        }
    }

    return (
        <>
            <div className="counter-flex">
                <Button onClick={decrementar} variant="secondary">-</Button>
                <p> {contador} </p>
                <Button onClick={incrementar} variant="secondary">+</Button>

            </div>
            <Button className="add-button" onClick={() => funcionAgregar(contador)} variant="dark">Agregar al carrito</Button>{' '}
            
        </>
    )
}

export default Counter