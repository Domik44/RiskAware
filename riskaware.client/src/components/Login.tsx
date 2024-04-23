﻿import { FormEvent, useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import { useAuth } from '../auth/useAuth';

// todo fix error on login
const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [loginError, setLoginError] = useState(false);
  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.isLoggedIn) {
      navigate('/');
    }
  }, [auth.isLoggedIn, navigate]);

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    try {
      await auth.login(email, password, rememberMe);
    }
    catch (error) {
      setLoginError(true); // Set login error state
    }
  };

  // todo split to Login a LoginForm
  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-6">
          <div className="card">
            <div className="card-body">
              <h2 className="text-center mb-4">Přihlášení</h2>
              <hr />
              {loginError && ( // Display error message if loginError is true
                <div className="alert alert-danger mt-3" role="alert">
                  <div className="text-center text-danger">
                    Přihlášení se nezdařilo, nesprávné přihlašovací údaje.
                  </div>
                </div>
              )}
              <form onSubmit={handleSubmit}>
                <div className="form-group my-3">
                  <label htmlFor="email" className="form-label">E-mail:</label>
                  {loginError && (
                    <input type="email" id="email" className="form-control is-invalid" value={email} placeholder="E-mail"
                      onChange={e => setEmail(e.target.value)} required />
                  )}
                  {!loginError && (
                    <input type="email" id="email" className="form-control" value={email} placeholder="E-mail"
                        onChange={e => setEmail(e.target.value)} required />
                  )}
                </div>
                <div className="form-group mb-3">
                  <label htmlFor="password" className="form-label">Heslo:</label>
                  {loginError && (
                    <input type="password" id="password" className="form-control is-invalid" value={password} placeholder="Heslo"
                      onChange={e => setPassword(e.target.value)} required />
                  )}
                  {!loginError && (
                  <input type="password" id="password" className="form-control" value={password} placeholder="Heslo"
                      onChange={e => setPassword(e.target.value)} required />
                  )}
                </div>
                <div className="form-check mb-3">
                  <input type="checkbox" id="rememberMe" className="form-check-input" checked={rememberMe}
                    onChange={e => setRememberMe(e.target.checked)} />
                  <label htmlFor="rememberMe" className="form-check-label">Zapamatovat heslo</label>
                </div>
                <div className="form-group d-flex align-items-center justify-content-center mt-4">
                  <button type="submit" className="btn btn-primary btn-lg">Přihlásit se</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
