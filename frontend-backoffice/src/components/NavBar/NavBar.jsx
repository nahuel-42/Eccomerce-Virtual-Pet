import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { FiLogOut, FiLogIn } from 'react-icons/fi';
import './NavBar.css';

function NavBar({ isLoggedIn, onLogout }) {
    const handleLogout = () => {
        if (onLogout) onLogout();
        window.location.href = '/login';
    };

    return (
        <Navbar expand="md" className="py-3 bg-dark text-light" variant="dark">
            <Container>
                <Navbar.Brand href="/" className="logo">Virtual Pet</Navbar.Brand>
                <Navbar.Toggle aria-controls="navbar-nav" />
                <Navbar.Collapse id="navbar-nav">
                    <Nav className="me-auto align-items-center miNav">
                        <Nav.Link href="/" className="nav-link">Inicio</Nav.Link>
                        <Nav.Link href="/pedidos" className="nav-link">Órdenes</Nav.Link>
                    </Nav>
                    <Nav className="align-items-center">
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
