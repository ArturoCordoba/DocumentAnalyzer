import React, { Component } from "react";
import { Link, withRouter } from "react-router-dom";

import { Form, Button, Row, Col, Alert } from "react-bootstrap";
import AuthBackground from "../auth-background/auth-background";

class SignUp extends Component {
  constructor(props) {
    super(props);

    this.state = {
      errorMessage: null,
      name: "",
      email: "",
      password1: "",
      password2: "",
    };

    this.handleNameChange = this.handleNameChange.bind(this);
    this.handleEmailChange = this.handleEmailChange.bind(this);
    this.handlePassword1Change = this.handlePassword1Change.bind(this);
    this.handlePassword2Change = this.handlePassword2Change.bind(this);
    this.onSubmit = this.onSubmit.bind(this);
  }

  onSubmit(e) {
    e.preventDefault();

    if (this.validateInputs()) {
      /** Solicitud HTTP */
    }
    this.props.history.push('/login');
  }

  /**
   * Funcion que valida los datos ingresados en el formulario
   * @returns False: datos invalidos, True: datos validos
   */
  validateInputs() {
    this.setState({ errorMessage: null });

    const name = this.state.name;
    const email = this.state.email;
    const password1 = this.state.password1;
    const password2 = this.state.password2;

    if (name === "" || email === "" || password1 === "" || password2 === "") {
      this.setState({
        errorMessage: "Por favor ingrese toda la información solicitada.",
      });
      return false;
    }

    if (password1 !== password2) {
      this.setState({
        errorMessage: "Las contraseñas no coinciden.",
      });
      return false;
    }

    return true;
  }

  render() {
    return (
      <>
        <AuthBackground>
          <h2 className="h4">Registro</h2>
          {this.state.errorMessage && (
            <Alert variant="danger">{this.state.errorMessage}</Alert>
          )}
          <Form onSubmit={this.onSubmit}>
            <Form.Group>
              <Form.Label>Nombre completo</Form.Label>
              <Form.Control
                type="text"
                placeholder="Nombre completo"
                onChange={this.handleNameChange}
              />
            </Form.Group>
            <Form.Group>
              <Form.Label>Correo electrónico</Form.Label>
              <Form.Control
                type="email"
                placeholder="Correo electrónico"
                onChange={this.handleEmailChange}
              />
            </Form.Group>
            <Form.Group>
              <Form.Label>Contraseña</Form.Label>
              <Form.Control
                type="password"
                placeholder="Contraseña"
                onChange={this.handlePassword1Change}
              />
            </Form.Group>
            <Form.Group>
              <Form.Label>Repetir Contraseña</Form.Label>
              <Form.Control
                type="password"
                placeholder="Contraseña"
                onChange={this.handlePassword2Change}
              />
            </Form.Group>
            <Row>
              <Col>
                <Link to="/" className="btn btn-secondary btn-block">
                  Cancelar
                </Link>
              </Col>
              <Col>
                <Button variant="primary" type="submit" block>
                  Crear usuario
                </Button>
              </Col>
            </Row>
          </Form>
        </AuthBackground>
      </>
    );
  }

  handleNameChange(e) {
    this.setState({ name: e.target.value });
  }

  handleEmailChange(e) {
    this.setState({ email: e.target.value });
  }

  handlePassword1Change(e) {
    this.setState({ password1: e.target.value });
  }

  handlePassword2Change(e) {
    this.setState({ password2: e.target.value });
  }
}

export default withRouter(SignUp);
