import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import ItemDetail from "../ItemDetail/ItemDetail";

const ItemDetailContainer = () => {
  const [producto, setProducto] = useState(null);
  const { idItem } = useParams();

  useEffect(() => {
    // Cargamos los productos guardados en localStorage
    const productosGuardados = JSON.parse(localStorage.getItem("productos")) || [];

    // Buscamos el producto cuyo id coincida con el idItem del parámetro
    const productoEncontrado = productosGuardados.find(prod => prod.id === Number(idItem));

    setProducto(productoEncontrado);
  }, [idItem]);

  if (!producto) {
    return <div>Cargando...</div>; // Opcional: mientras no encuentra el producto
  }

  return (
    <div>
      <ItemDetail {...producto} />
    </div>
  );
};

export default ItemDetailContainer;
