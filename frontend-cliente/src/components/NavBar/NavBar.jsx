import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { FiLogOut, FiLogIn } from 'react-icons/fi';
import { useAuth } from '../../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import './NavBar.css';
import CartWidget from '../CartWidget/CartWidget';

function NavBar() {
    const { logout, isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
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
                        <Nav.Link className="nav-link" href="/orders">Mis Pedidos</Nav.Link>
                    </Nav>
                    <Nav className="align-items-center">
                        <CartWidget/>
                        {isAuthenticated ? (
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
