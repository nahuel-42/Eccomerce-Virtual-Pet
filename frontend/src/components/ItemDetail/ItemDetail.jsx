import React from 'react'
import './ItemDetail.css';
import { Link } from 'react-router-dom'
import { ChartContext } from '../../context/ChartContext';
import { useContext, useState, useEffect } from 'react';
import Counter from '../Counter/Counter';
import Button from 'react-bootstrap/Button';
import Toast from 'react-bootstrap/Toast';

const ItemDetail = ({ id, nombre, precio, img, stock, desc }) => {
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
    const item = { id, nombre, precio };
    agregarAlCarrito(item, cantidad);

  }

  return (
    <div className='contenedor-item'>

      <img src={img} alt={nombre} />

      <div className='info-prod'>
        <h2> {nombre} </h2>
        <h3> $ {precio} </h3>
        <span className='span'> </span>
        {/* <p>Stock: {stock} </p> */}
        {
          agregarCantidad > 0 ? (<div className='agrega-prod'><Link to="/cart"><Button className='boton-fin' variant="dark">Terminar compra</Button></Link><Link to="/"><Button variant="dark">Ver más productos</Button> </Link></div>) : (<div><Counter inicial={1} stock={stock} funcionAgregar={manejadorCantidad} /><p> {desc} </p></div>)
        }
        {mostrarToast && (
          <Toast
          show={mostrarToast}
          onClose={() => setMostrarToast(false)}
          className="position-absolute top-0 end-0 mt-2 mr-2"
        >
            <Toast.Header>
              <img src="holder.js/20x20?text=%20" className="rounded me-2" alt="" />
              <strong className="me-auto">Listo!</strong>
              <small>:)</small>
            </Toast.Header>
            <Toast.Body>Ya tenés tu producto en el carrito!  </Toast.Body>
          </Toast>
        )}

      </div>



    </div>
  )
}


export default ItemDetail