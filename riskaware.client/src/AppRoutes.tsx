import { AllProjectList } from "./components/AllProjectList";
import { MyProjectList } from "./components/MyProjectList";
import { ProjectDetail } from "./components/ProjectDetail";
import { AllProjectsMaterial } from "./components/AllProjectsMaterial";
import Login from './components/Login';

const AppRoutes = [
  {
    path: '/',
    element: <AllProjectList />,
    isProtected: true
  },
  {
    path: '/myProjects',
    element: <MyProjectList />,
    isProtected: true
  },
  {
    path: '/project/:id',
    element: <ProjectDetail />,
    isProtected: true
  },
  {
    path: '/allProjectsMaterial',
    element: <AllProjectsMaterial />,
    isProtected: true
  },
  {
    path: '/login',
    element: <Login />,
    isProtected: false
  },
  //{
  //  path: '/project/:id/risk/:riskId', // TODO -> mby delete
  //  element: <ProjectDetail />,
  //  isProtected: true
  //}
];

export default AppRoutes;
