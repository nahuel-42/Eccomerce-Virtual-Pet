import React from 'react'
import './CartItem.css';
const CartItem = ({item, cantidad}) => {
  return (
    <div>
        <h3 className='prod-nombre'> {item.nombre}  </h3>
        <p> Cantidad: {cantidad} </p>
        <p> Precio: {item.precio} </p>
    </div>
  )
}

export default CartItem