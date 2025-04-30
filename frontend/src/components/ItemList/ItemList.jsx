// src/components/ItemList/ItemList.jsx
import React from 'react';
import './ItemList.css';

const ItemList = ({ productos }) => {
  return (
    <div className="item-list justify-content-center">
      {productos.length > 0 ? (
        productos.map((producto) => (
          <div key={producto.id} className="card text-start ">
            <img src={producto.imageUrl} alt={producto.name} className="card-img-top" />
            <div className="card-body align-text-bottom mt-3">
              <h4 className='card-title'>{producto.name}</h4>
              <p className='card-text'>${producto.price}</p>
              <button className='btn btn-success'>Ver detalle</button>
            </div>
          </div>
        ))
      ) : (
        <p>No se encontraron productos.</p>
      )}
    </div>
  );
};

export default ItemList;