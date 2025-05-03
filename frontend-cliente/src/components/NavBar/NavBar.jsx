import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { FiLogOut, FiLogIn } from 'react-icons/fi';
import './NavBar.css';
import CartWidget from '../CartWidget/CartWidget';

function NavBar({ isLoggedIn, onLogout }) {
    const handleLogout = () => {
        if (onLogout) onLogout(); 
        localStorage.removeItem('token'); 
        navigate('/login'); 
    };


    return (
        <Navbar expand="md" className="bg-dark text-light py-3" variant="dark">
            <Container>
                <Navbar.Brand href="/" className="logo">Virtual Pet</Navbar.Brand>
                <Navbar.Toggle aria-controls="navbar-nav" />
                <Navbar.Collapse id="navbar-nav">
                    <Nav className="me-auto align-items-center miNav">
                        <Nav.Link href="/" className="nav-link">Inicio</Nav.Link>
                        <NavDropdown
                            title="Categorías"
                            id="nav-categorias"
                            className="nav-link"
                            menuClassName="multi-col-dropdown"
                        >
                            <div className="dropdown-grid">
                                <div>
                                    <div className="dropdown-header">Categorías por Animal</div>
                                    <NavDropdown.Item href="/category/perro">Perro</NavDropdown.Item>
                                    <NavDropdown.Item href="/category/gato">Gato</NavDropdown.Item>
                                    <NavDropdown.Item href="/category/pajaro">Pájaro</NavDropdown.Item>
                                </div>
                            </div>
                        </NavDropdown>
                        <Nav.Link className="nav-link" href="/orders">Mis Pedidos</Nav.Link>
                    </Nav>
                    <Nav className="align-items-center">
                        <CartWidget/>
                        {isLoggedIn ? (
                            <Nav.Link onClick={handleLogout} className="nav-link d-flex align-items-center">
                                <FiLogOut size={20} className="me-1" />
                            </Nav.Link>
                        ) : (
                            <Nav.Link href="/login" className="nav-link d-flex align-items-center">
                                <FiLogIn size={20} className="me-1" />
                            </Nav.Link>
                        )}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

export default NavBar;
