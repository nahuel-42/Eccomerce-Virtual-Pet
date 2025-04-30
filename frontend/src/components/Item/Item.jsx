import React from 'react'
import './Item.css'
import { Link } from 'react-router-dom'
import Button from 'react-bootstrap/Button';

//componente para mostrar informacion 
const Item = ({id, name, price, imageUrl, }) => {

  return (
      <div key={id} className="card text-start ">
            <img src={imageUrl} alt={name} className="card-img-top" />
            <div className="card-body align-text-bottom mt-3">
              <h4 className='card-title'>{name}</h4>
              <p className='card-text'>${price}</p>
              <Link to={`/item/${id}`}><button className="btn btn-success" size="sm">
                Ver detalles
              </button></Link>
            </div>
      </div>
  )
}

export default Item