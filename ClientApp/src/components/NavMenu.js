import React, {useContext, useEffect, useState} from 'react';
import { Link, useNavigate } from 'react-router-dom'
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import './NavMenu.css';
import { TokenContext } from "../App";

export function NavMenu(){
    const {  setUsername } = useContext(TokenContext);
    let username = sessionStorage.getItem("todoUsername")
    let navigate = useNavigate()
    let token = sessionStorage.getItem("todoJWT");
    const [expanded, setExpanded] = useState(false);

    function logout(){
        sessionStorage.setItem("todoJWT", "")
        sessionStorage.setItem("todoUsername", "")
        setUsername("")
        navigate("/login")
    }

  useEffect(() => {
      if (username === ""){
          setUsername(sessionStorage.getItem("todoUsername"))
      }
  }, [setUsername, username])

    return (
        <Navbar expand="lg" expanded={expanded}>
            <Container>
                <Navbar.Toggle onClick={() => setExpanded(expanded ? false : "expanded")} aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Link to="/" className="nav-item nav-link active" onClick={() => setExpanded(false)}>All lists</Link>
                        <Link to="/todos/archived" className="nav-item nav-link active" onClick={() => setExpanded(false)}>Archived lists</Link>
                        <Link to="/items/today" className="nav-item nav-link active" onClick={() => setExpanded(false)}>Today</Link>
                        <Link to="/items/priority" className="nav-item nav-link active" onClick={() => setExpanded(false)}>Priority</Link>
                        {token === "" ? "" : <Nav.Link className="nav-item nav-link logout active" onClick={() => {setExpanded(false); logout()}}>Logout</Nav.Link>}
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}
