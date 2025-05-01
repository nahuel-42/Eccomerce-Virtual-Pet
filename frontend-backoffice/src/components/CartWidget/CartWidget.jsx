import { useContext } from 'react';
import { ChartContext } from '../../context/ChartContext';
import { Link } from 'react-router-dom';
import './CartWidget.css'

const CartWidget = () => {

    const { cantidadTotal } = useContext(ChartContext);

    return (
        <Link to='/cart' style={{ color: 'black', textDecoration: 'none' }}>
            <div className='divCart'>
                <span className="material-symbols-outlined" style={{ color: 'black' }} >
                    shopping_cart
                </span>
             
                {
                    cantidadTotal > 0 && <p style={{ textDecoration: 'underline' }}> { cantidadTotal } </p>
                }
            </div>
        </Link>
    )
}

export default CartWidget