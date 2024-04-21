import { createContext, useState, useEffect, ReactNode } from 'react';
import { useNavigate } from "react-router-dom";

interface AuthContextType {
  isLoggedIn: boolean;
  isLoading: boolean;
  email: string;
  login: (email: string, password: string, rememberMe: boolean) => Promise<void>;
  logout: () => void;
}

interface AuthProviderProps {
  children: ReactNode;
}

interface ILoginResponse {
  isLoggedIn: boolean;
  email: string;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [email, setEmail] = useState('');

  useEffect(() => {
    setIsLoading(true);
    const checkIsLoggedIn = async () => {
      try {
        const response = await fetch("/api/Account/IsLoggedIn");
        const data: ILoginResponse = await response.json();
        setIsLoggedIn(data.isLoggedIn);
        setEmail(data.email);
      }
      catch (error) {
        console.error("Failed to check login status", error);
        setIsLoggedIn(false);
      }
      finally {
        setIsLoading(false);
      }
    };

    checkIsLoggedIn();
  }, []);

  const login = async (email: string, password: string, rememberMe: boolean) => {
    setIsLoading(true);
    const loginData = {
      email,
      password,
      rememberMe
    };

    const response = await fetch("/api/Account/login", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
      body: JSON.stringify(loginData),
    });

    if (response.ok) {
      setIsLoggedIn(true);
      setEmail(email);
      navigate('/');
    }
    else {
      throw new Error('Login failed');
    }
    setIsLoading(false);
  };

  const logout = async () => {
    try {
      const response = await fetch("/api/Account/logout", {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
      });
      if (response.ok) {
        setIsLoggedIn(false);
        setEmail(email);
        navigate('/login');
      }
      else {
        console.error('Logout failed');
      }
    }
    catch (error) {
      console.error('Failed to logout', error);
    }
    finally {
      setIsLoading(false);
    }
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, isLoading, email, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
