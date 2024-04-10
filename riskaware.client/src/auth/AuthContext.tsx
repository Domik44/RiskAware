import { createContext, useState, useEffect, ReactNode } from 'react';
import { API_URL } from "./constants.tsx";
import { useNavigate } from "react-router-dom";

interface AuthContextType {
  isLoggedIn: boolean;
  isLoading: boolean;
  login: (email: string, password: string, rememberMe: boolean) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setIsLoading(true);
    const checkIsLoggedIn = async () => {
      try {
        const response = await fetch(`${API_URL}/api/Account/IsLoggedIn`, {
          credentials: 'include',
        });
        const data = await response.json();
        setIsLoggedIn(data.isLoggedIn);
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

    const response = await fetch(`${API_URL}/api/Account/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
      body: JSON.stringify(loginData),
    });

    if (response.ok) {
      setIsLoggedIn(true);
      navigate('/');
    }
    else {
      throw new Error('Login failed');
    }
    setIsLoading(false);
  };

  const logout = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Account/logout`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
      });
      if (response.ok) {
        setIsLoggedIn(false);
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
    <AuthContext.Provider value={{ isLoggedIn, isLoading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
