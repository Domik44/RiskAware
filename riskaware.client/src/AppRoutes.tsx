import { ProjectList } from "./components/ProjectList";
import { Forecast } from "./components/Forecast";
import Login from './components/Login';

const AppRoutes = [
  {
    path: '/',
    element: <ProjectList />,
    isProtected: true
  },
  {
    path: '/forecast',
    element: <Forecast />,
    isProtected: true
  },
  {
    path: '/login',
    element: <Login />,
    isProtected: false
  }
];

export default AppRoutes;
