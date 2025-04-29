import React from 'react'
import './Item.css'
import { Link } from 'react-router-dom'
import Button from 'react-bootstrap/Button';

//componente para mostrar informacion 
const Item = ({id, nombre, precio, img}) => {

  return (
    <div className='cardProducto'>
        <img src={img} alt={nombre} />
        <h3>{nombre}</h3>
        {/* <p>{desc}</p> */}
        <p>Precio: $ {precio}</p>
        <Link to={`/item/${id}`}><Button variant="secondary" size="sm">
          Ver detalles
        </Button></Link>
    </div>
  )
}

export default Item