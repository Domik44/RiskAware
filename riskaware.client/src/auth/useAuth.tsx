import { useContext } from 'react';
import AuthContext from './AuthContext';

// Hook (useX) for using the AuthContext
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === null) {
    throw new Error("Hook useAuth must be used inside AuthProvider");
  }
  return context;
};
