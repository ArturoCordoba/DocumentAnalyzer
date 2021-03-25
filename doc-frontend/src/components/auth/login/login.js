import React, { Component } from "react";
import { Link } from "react-router-dom";

import { Form, Button, Alert } from "react-bootstrap";
import AuthBackground from "../auth-background/auth-background";

class LogIn extends Component {
  onSubmit(e) {
    e.preventDefault();
    console.log(e);
  }

  render() {
    return (
      <>
        <AuthBackground>
          <h2 className="h4">Iniciar sesión</h2>
          <Form onSubmit={this.onSubmit}>
            <Form.Group>
              <Form.Label>Correo electrónico</Form.Label>
              <Form.Control type="email" placeholder="Correo electrónico" />
            </Form.Group>
            <Form.Group>
              <Form.Label>Contraseña</Form.Label>
              <Form.Control type="password" placeholder="Contraseña" />
            </Form.Group>
            <Button variant="primary" type="submit" block>
              Ingresar
            </Button>
          </Form>
          <Alert variant="info" className="mt-3">
            ¿Nuevo usuario? <Link to="/signup" className="alert-link">Regístrese aquí</Link>
          </Alert>
        </AuthBackground>
      </>
    );
  }
}

export default LogIn;
