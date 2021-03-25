import React from "react";
import { Switch } from "react-router-dom";
import Route from "./Route";

import LogIn from "../components/auth/login/login"
import Signup from "../components/auth/signup/signup"

export default function Routes() {
  return (
    <Switch>
      <Route path="/" exact component={LogIn} />
      <Route path="/signup" exact component={Signup} />


      {/* redirect user to SignIn page if route does not exist and user is not authenticated */}
      <Route component={LogIn} />
    </Switch>
  );
  //<Route path="/dashboard" component={Dashboard} isPrivate />
}
