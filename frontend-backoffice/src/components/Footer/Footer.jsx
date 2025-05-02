import React from "react";

const Footer = () => {
  return (
    <footer className="bg-dark text-light py-5 mt-auto">
      <div className="container">
        <div className="row text-center text-md-start">
        <div className="col-md-4 mb-4 mb-md-0">
            <h5 className="mb-2">VirtualPet 🐾</h5>
            <p className="fst-italic">VirtualPet nunca defraudará a su mascota.</p>
            <p>Tu tienda online de confianza para todo lo que tus mascotas necesitan.</p>
        </div>


          <div className="col-md-4 mb-4 mb-md-0">
            <h5 className="mb-3">Enlaces útiles</h5>
            <ul className="list-unstyled">
              <li><a href="/" className="text-light text-decoration-none">Inicio</a></li>
              <li><a href="/productos" className="text-light text-decoration-none">Productos</a></li>
              <li><a href="/contacto" className="text-light text-decoration-none">Contacto</a></li>
              <li><a href="/nosotros" className="text-light text-decoration-none">Sobre nosotros</a></li>
            </ul>
          </div>

          <div className="col-md-4">
            <h5 className="mb-3">Seguinos</h5>
            <div className="d-flex justify-content-center justify-content-md-start gap-3">
              <a href="#" className="text-light fs-4"><i className="bi bi-facebook"></i></a>
              <a href="#" className="text-light fs-4"><i className="bi bi-instagram"></i></a>
              <a href="#" className="text-light fs-4"><i className="bi bi-twitter-x"></i></a>
              <a href="#" className="text-light fs-4"><i className="bi bi-youtube"></i></a>
            </div>
          </div>
        </div>

        <hr className="my-4 border-light" />
        <p className="text-center mb-0">&copy; 2025 VirtualPet. Todos los derechos reservados.</p>
      </div>
    </footer>
  );
};

export default Footer;
