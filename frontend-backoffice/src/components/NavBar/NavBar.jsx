import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { useAuth } from '../../context/AuthContext';
import { FiLogOut, FiLogIn } from 'react-icons/fi';
import './NavBar.css';
import { useNavigate } from 'react-router-dom';

function NavBar() {
    const { logout, isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <Navbar expand="md" className="py-3 bg-dark text-light" variant="dark">
            <Container>
                <Navbar.Brand href="/" className="logo">Virtual Pet</Navbar.Brand>
                <Navbar.Toggle aria-controls="navbar-nav" />
                <Navbar.Collapse id="navbar-nav">
                    <Nav className="me-auto align-items-center miNav">
                        <Nav.Link href="/" className="nav-link">Órdenes</Nav.Link>
                        <Nav.Link href="/register" className="nav-link">Registrar Usuarios</Nav.Link>
                    </Nav>
                    <Nav className="align-items-center">
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
