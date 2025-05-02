import React from 'react'
import './CartItem.css';
const CartItem = ({item, cantidad}) => {
  return (
    <div className='fw-medium fs-6 justify-content-between d-flex flex-row px-2 py-1 '>
        <p className='me-3'>{item.name}</p>

        <div className="d-flex flex-row">
          <p className='text-secondary'>{cantidad} x</p>
          <p className='mx-2 fw-semibold'>  $ {item.price} </p>
        </div>
    </div>
  )
}

export default CartItem