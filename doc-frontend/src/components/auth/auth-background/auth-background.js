import React, { Component } from "react";
import { Col, Row } from "react-bootstrap";
import "./auth-background.css";


class AuthBackground extends Component {
  render() {
    return (
      <>
        <div className="bg-image">
          <Row className="justify-content-center align-items-center h-100">
            <Col sm="4" className="card text-white p-4">
              {this.props.children}
            </Col>
          </Row>
        </div>
      </>
    );
  }
}

export default AuthBackground;
