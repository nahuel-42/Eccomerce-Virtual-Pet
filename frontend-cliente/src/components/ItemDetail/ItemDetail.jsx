import React from 'react'
import './ItemDetail.css';
import { Link } from 'react-router-dom'
import { ChartContext } from '../../context/ChartContext';
import { useContext, useState, useEffect } from 'react';
import Counter from '../Counter/Counter';
import Toast from 'react-bootstrap/Toast';

const ItemDetail = ({ id, name, price, imageUrl, description }) => {
  //Creamos  un estado local con la cantidad de productos agregados. 
  const [agregarCantidad, setAgregarCantidad] = useState(0);
  const { agregarAlCarrito } = useContext(ChartContext);
  const [mostrarToast, setMostrarToast] = useState(false);

  useEffect(() => {
    if (agregarCantidad > 0) {
      setMostrarToast(true);
      const timeoutId = setTimeout(() => {
        setMostrarToast(false);
      }, 3000);
      return () => clearTimeout(timeoutId);
    }
  }, [agregarCantidad]);


  const manejadorCantidad = (cantidad) => {

    setAgregarCantidad(cantidad);
    //voy a crear un objeto con el item y la cantidad
    const item = { id, name, price };
    agregarAlCarrito(item, cantidad);
  }

  return (
    <div className="card mb-3">
      <div className="row g-0">
        <div className="col-md-5">
          <img className="img-fluid rounded-start" src={imageUrl} alt={name} />
        </div>
        <div className="col-md-7">
          <div className="card-body h-100 d-flex flex-column justify-content-between align-items-center">
            <div className='text-center'>
              <h2 className="card-title">{name}</h2>
              <p className="card-text">{description}</p>
            </div>
            <p className='fs-1 fw-semibold text-success'> $ {price} </p>
            <div className='info-prod w-100 d-flex flex-column align-items-center'>
              {
                agregarCantidad > 0 ? (
                  <div className='d-flex flex-column w-40'>
                    <Link to="/cart">
                      <button className='btn btn-dark w-100'>Ver carrito</button>
                    </Link>
                    <Link to="/">
                      <button className='btn btn-secondary mt-2 w-100'>Seguir comprando</button> 
                    </Link>
                  </div>) 
                : (
                    <div><Counter inicial={1} funcionAgregar={manejadorCantidad} /></div>)
              }
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}


export default ItemDetail