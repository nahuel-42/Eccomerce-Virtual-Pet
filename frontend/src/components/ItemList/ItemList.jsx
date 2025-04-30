// src/components/ItemList/ItemList.jsx
import React from 'react';
import './ItemList.css';
import Item from '../Item/Item.jsx'

const ItemList = ({ productos }) => {
  return (
    <div className="item-list justify-content-center">
      {productos.length > 0 ? (        
        productos.map(prod => <Item key={prod.id} {...prod} ></Item>)
      ) : (
        <p>No se encontraron productos.</p>
      )}
    </div>
  );
};

export default ItemList;