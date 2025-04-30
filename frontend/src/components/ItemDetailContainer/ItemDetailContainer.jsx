import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import ItemDetail from "../ItemDetail/ItemDetail";
import './ItemDetailContainer.css';
import { fetchProduct } from '../../services/product.service';

const ItemDetailContainer = () => {
  const [producto, setProducto] = useState(null);
  const { idItem } = useParams();

  useEffect(() => {
    async function loadProduct() {
      const productoDetalle = await fetchProduct(idItem);
      console.log("🚀 ~ loadProduct ~ productoDetalle:", productoDetalle)
      console.log("🚀 ~ loadProduct ~ idItem:", idItem)

      setProducto(productoDetalle);
    }
    
    loadProduct();

  }, [idItem]);

  if (!producto) {
    return <div>Cargando...</div>; // Opcional: mientras no encuentra el producto
  }

  return (
    <div className="detail-container">
      <ItemDetail {...producto} />
    </div>
  );
};

export default ItemDetailContainer;
