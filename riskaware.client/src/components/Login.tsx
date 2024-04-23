import { FormEvent, useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import { useAuth } from '../auth/useAuth';
import PersonIcon from '@mui/icons-material/Person';
import LockIcon from '@mui/icons-material/Lock';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
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
      setLoginError(false);
      await auth.login(email, password, rememberMe);
    }
    catch (error) {
      setLoginError(true); // Set login error state
    }
  };

  const removeError = () => {
    if (loginError) {
      setLoginError(false);
    }
  }

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-6">
          <div className="card">
            <div className="card-body">
              <h3 className="text-center mb-4">Přihlášení</h3>
              <hr />
              {loginError && ( // Display error message if loginError is true
                <div className="alert alert-danger mt-3" role="alert">
                  <div className="text-center text-danger">
                    Přihlášení se nezdařilo, nesprávné přihlašovací údaje.
                  </div>
                </div>
              )}
              <form onSubmit={handleSubmit} onChange={removeError}>
                <div className="form-row">
                  <div className="col-12">
                    <label htmlFor="email" className="form-label">E-mail:</label>
                    <div className="input-group mb-2">
                      <span className="input-group-text">
                        <PersonIcon />
                      </span>
                      <input
                        type="email"
                        id="email"
                        name="email"
                        className="form-control"
                        value={email}
                        placeholder="E-mail"
                        onChange={e => setEmail(e.target.value)}
                        required />
                    </div>
                  </div>
                  <div className="col-12">
                    <label htmlFor="password" className="form-label">Heslo:</label>
                    <div className="input-group mb-2">
                      <div className="input-group-prepend">
                        <span className="input-group-text">
                          <LockIcon />
                        </span>
                      </div>
                      <input
                        type={showPassword ? "text" : "password"}
                        id="password"
                        name="password"
                        className="form-control"
                        value={password}
                        placeholder="Heslo"
                        onChange={e => setPassword(e.target.value)}
                        required />
                      <div className="input-group-append">
                        <span className="input-group-text" onClick={togglePasswordVisibility}>
                          {showPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
                        </span>
                      </div>
                    </div>
                  </div>
                  <div className="form-check my-3">
                    <input
                      type="checkbox"
                      id="rememberMe"
                      name="rememberMe"
                      className="form-check-input"
                      checked={rememberMe}
                      onChange={e => setRememberMe(e.target.checked)} />
                    <label htmlFor="rememberMe" className="form-check-label">Zapamatovat heslo</label>
                  </div>
                  <div className="form-group d-flex align-items-center justify-content-center mt-4">
                    <button type="submit" className="btn btn-primary btn-lg">Přihlásit se</button>
                  </div>
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
