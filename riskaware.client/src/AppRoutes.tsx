import { AllProjectsList } from "./components/ProjectLists/AllProjectsList";
import { MyProjectsList } from "./components/ProjectLists/MyProjectsList";
import { ProjectDetail } from "./components/ProjectDetail";
import Login from './components/Login';

const AppRoutes = [
  {
    path: '/',
    element: <AllProjectsList />,
    isProtected: true
  },
  {
    path: '/myProjects',
    element: <MyProjectsList />,
    isProtected: true
  },
  {
    path: '/project/:id',
    element: <ProjectDetail />,
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
