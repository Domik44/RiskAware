import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { AuthProvider } from './auth/AuthContext.tsx';
import ProtectedRoute from './auth/ProtectedRoute.tsx';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

function App() {
  return (
    <Router>
      <AuthProvider>
        <Layout>
          <Routes>
            {AppRoutes.map((route, index) => {
              const Component = route.element;
              return (
                <Route
                  key={index}
                  path={route.path}
                  element={
                    route.isProtected ?
                      <ProtectedRoute>{Component}</ProtectedRoute> :
                      Component
                  }
                />
              );
            })}
          </Routes>
        </Layout>
      </AuthProvider>
    </Router>
  );
}

export default App;
