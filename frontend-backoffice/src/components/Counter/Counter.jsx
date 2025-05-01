import { useState, useEffect } from "react";
import Button from 'react-bootstrap/Button';
import './Counter.css';

const Counter = ({ inicial, funcionAgregar }) => {

    const [contador, setContador] = useState(1);

    const incrementar = () => {
        setContador(contador + 1);
    }

    const decrementar = () => {
        if (contador > inicial) {
            setContador(contador - 1);
        }
    }

    return (
        <>
            <div className="counter-flex">
                <button onClick={decrementar} className="btn btn-secondary">-</button>
                <p> {contador} </p>
                <button onClick={incrementar} className="btn btn-secondary">+</button>

            </div>
            <button className="btn btn-dark w-100" onClick={() => funcionAgregar(contador)} >Agregar al carrito</button>{' '}
            
        </>
    )
}

export default Counter