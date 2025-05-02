import { useEffect } from "react";

const CargarArray = () => {
  useEffect(() => {
    const productos = [
      { id: 1, nombre: 'Comida Perros', precio: 1200, img: 'https://www.istockphoto.com/photo/dry-pet-food-gm102743103-12622792', idCat: 'alimentos', stock: 50, desc: 'Alimento premium balanceado para perros.' },
      { id: 2, nombre: 'Comida Gatos', precio: 1100, img: 'https://www.istockphoto.com/photo/dry-pet-food-gm102743103-12622792?phrase=cat%20food%20bag', idCat: 'alimentos', stock: 60, desc: 'Alimento completo para gatos adultos.' },
      { id: 3, nombre: 'Cama Perro', precio: 2500, img: 'https://www.amazon.com/Lunarable-Continuous-Monochrome-Background-Resistant/dp/B08RDH9Y7K', idCat: 'accesorios', stock: 20, desc: 'Cama acolchada para perros medianos.' },
      { id: 4, nombre: 'Juguete Aves', precio: 600, img: 'https://www.amazon.com/GATMAHE-Cockatoo-Climbing-Unraveling-Preening/dp/B095PKK93P', idCat: 'accesorios', stock: 30, desc: 'Juguete interactivo para aves.' },
      { id: 5, nombre: 'Suplemento Vitaminas', precio: 900, img: 'https://www.dreamstime.com/illustration/pet-supplement.html', idCat: 'salud', stock: 40, desc: 'Suplemento multivitamínico para mascotas.' },
      { id: 6, nombre: 'Shampoo Mascotas', precio: 700, img: 'https://www.dreamstime.com/illustration/bottle-pet-shampoo.html', idCat: 'salud', stock: 35, desc: 'Shampoo natural para pelaje suave.' },
      { id: 7, nombre: 'Suéter Perro', precio: 1800, img: 'https://canadapooch.com/products/soho-sweater', idCat: 'ropa', stock: 25, desc: 'Suéter de lana para perros pequeños.' },
      { id: 8, nombre: 'GPS Collar', precio: 5500, img: 'https://www.amazon.com/ANSION-Real-Time-Position-Tracking-Waterproof/dp/B0D9B25K3K', idCat: 'tecnologia', stock: 10, desc: 'GPS para rastreo de mascotas.' },
      { id: 9, nombre: 'Pack Inicio', precio: 3500, img: 'https://www.amazon.com/Puppy-Starter-Piece-Supplies-Assortments/dp/B082R1N45Y', idCat: 'ofertas', stock: 15, desc: 'Pack de inicio con varios accesorios.' }
    ];

    console.log("Productos cargados:", productos);

    // OPCIONAL: guardarlos en localStorage para que persistan al recargar
    localStorage.setItem("productos", JSON.stringify(productos));
  }, []);

  return null;
};

export default CargarArray;
