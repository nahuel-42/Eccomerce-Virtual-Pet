import React, { useEffect, useState } from 'react';
import './ItemListContainere.css';
import ItemList from '../ItemList/ItemList';
import { useParams } from 'react-router-dom';
import { fetchProducts } from '../../services/product.service';

const ItemListContainer = () => {
  const [productos, setProductos] = useState([]);
  const { idCategory } = useParams();

  useEffect(() => {
    async function loadProducts() {
      const todosLosProductos = await fetchProducts();

      const productosFiltrados = idCategory
        ? todosLosProductos.filter(prod =>
            prod.animalCategories.some(cat => cat.id === parseInt(idCategory))
          )
        : todosLosProductos;

      setProductos(productosFiltrados);
    }

    loadProducts();
  }, [idCategory]);

  return (
    <div className='divList'>
      <ItemList productos={productos} />
    </div>
  );
};

export default ItemListContainer;
