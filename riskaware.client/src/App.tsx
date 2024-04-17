import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { AuthProvider } from './auth/AuthContext.tsx';
import ProtectedRoute from './auth/ProtectedRoute.tsx';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";

// Localization libraries
import { createTheme, ThemeProvider, useTheme } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3';
import { cs } from 'date-fns/locale';
import { csCZ } from '@mui/material/locale';

function App() {
  const theme = useTheme();
  return (
    <ThemeProvider theme={createTheme(theme, csCZ)}>
      <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={cs}>
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
      </LocalizationProvider>
    </ThemeProvider>
  );
}

export default App;
