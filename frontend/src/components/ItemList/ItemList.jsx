import React from 'react'
import Item from '../Item/Item'
import './ItemList.css';

const ItemList = ({productos}) => {
  return (
    <div className='gridList'>
        {/* voy a mapear la info que recibo por props. Por cada elemento de productos voy a generar un item. */}
        {productos.map(prod => <Item key={prod.id} {...prod} ></Item>)}
        <div></div>
    </div>
  )
}

export default ItemList