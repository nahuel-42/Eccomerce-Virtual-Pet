import { useState, useContext, useEffect } from "react";
import { ChartContext } from "../../context/ChartContext";
// import { db } from "../../services/config";
// import { collection, addDoc, updateDoc, getDoc, doc } from "firebase/firestore";
import './Checkout.css'
import { Button } from "react-bootstrap";
import Toast from 'react-bootstrap/Toast';


const Checkout = () => {

    const { carrito, vaciarCarrito, total } = useContext(ChartContext);
    const [nombre, setNombre] = useState("");
    const [apellido, setApellido] = useState("");
    const [telefono, setTelefono] = useState("");
    const [email, setEmail] = useState("");
    const [emailConfirmacion, setEmailConfirmacion] = useState("");
    const [ordenId, setOrdenId] = useState("");
    const [error, setError] = useState("");
    const [mostrarToast, setMostrarToast] = useState(false);


    const manejadorSubmit = (event) => {
        event.preventDefault();
        //Verificamos que todos los campos se completen: 
        if (!nombre || !apellido || !telefono || !email || !emailConfirmacion) {
            setError("¡Por favor completa todos los campos");
            return;
        }

        //Validamos que el email coincida: 
        if (email !== emailConfirmacion) {
            setError("Los emails no coinciden");
            return;
        }

        //Creamos un objeto con todos los datos de la orden: 
        const orden = {
            items: carrito.map(producto => ({
                id: producto.item.id,
                nombre: producto.item.nombre,
                cantidad: producto.cantidad
            })),
            total: total,
            fecha: new Date(),
            nombre,
            apellido,
            telefono,
            email
        }

        console.log('ORDEN: ' + JSON.stringify(orden));

        Promise.all(
            orden.items.map(async (productoOrden) => {
                console.log(typeof (productoOrden.id));
                //Por cada producto obtengo una referencia y a partir de esa referencia el doc. 
                const productoRef = doc(db, "items", productoOrden.id.toString());
                console.log('Producto ref ' + productoRef);
                const productoDoc = await getDoc(productoRef);
                console.log('Producto doc ' + productoDoc.data);
                const stockActual = productoDoc.data().stock;

                await updateDoc(productoRef, { stock: stockActual - productoOrden.cantidad });
                //Modifico el stock y subo la actualización. 
            })
        )//Guardamos en la base de datos la orden de compra: 
            .then(() => {
                addDoc(collection(db, "ordenes"), orden)
                    .then(docRef => {
                        setOrdenId(docRef.id);
                        vaciarCarrito();
                    })
                    .catch(error => console.log("Error al crear la orden", error))
            })
            .catch(error => {
                console.log("No pudimos actualizar el stock", error);
                setError("Error al actualizar el stock");
            })
    }

    return (
        <div>
            <h2>Checkout - Finalizamos la Compra </h2>

            <form onSubmit={manejadorSubmit}>
                {
                    carrito.map(producto => (
                        <div key={producto.item.id}>
                            <p> {producto.item.nombre} x {producto.cantidad} </p>
                            <p> {producto.item.precio} </p>
                            <hr />
                        </div>
                    ))
                }

                <div className="form-group">
                    <label htmlFor="nombre"> Nombre </label>
                    <input type="text" value={nombre} id="nombre" onChange={(e) => setNombre(e.target.value)} />
                </div>

                <div className="form-group">
                    <label htmlFor="apellido"> Apellido </label>
                    <input type="text" value={apellido} id="apellido" onChange={(e) => setApellido(e.target.value)} />
                </div>

                <div className="form-group">
                    <label htmlFor="telefono"> Telefono </label>
                    <input type="text" value={telefono} id="telefono" onChange={(e) => setTelefono(e.target.value)} />
                </div>

                <div className="form-group">
                    <label htmlFor="email"> E-mail </label>
                    <input type="email" value={email} id="email" onChange={(e) => setEmail(e.target.value)} />
                </div>

                <div className="form-group">
                    <label htmlFor="emailcon"> Email Confirmación </label>
                    <input type="email" value={emailConfirmacion} id="emailcon" onChange={(e) => setEmailConfirmacion(e.target.value)} />
                </div>

                {
                    error && <p style={{ color: "red" }}> {error} </p>
                }

                <div className="botones">
                    <Button type="submit" disabled={carrito.length === 0} variant="dark" onClick={() => {setError(''); setMostrarToast(true)}}>Finalizar Orden</Button>
                </div>

                {ordenId && (
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
                        <Toast.Body>Orden generada con éxito! Revisa tu casilla de correo. </Toast.Body>
                        <Toast.Body> <strong>Nro de orden: {ordenId} </strong> </Toast.Body>
                    </Toast>
                )}
                {
                    ordenId && <strong>¡Gracias por su compra {nombre}! Tu número de orden es el siguiente: {ordenId} </strong>
                }


            </form>
        </div>
    )
}

export default Checkout