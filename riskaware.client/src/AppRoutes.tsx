﻿import { ProjectList } from "./components/ProjectList";
import { MyProjectList } from "./components/MyProjectList";
import { ProjectDetail } from "./components/ProjectDetail";
import Login from './components/Login';

const AppRoutes = [
  {
    path: '/',
    element: <ProjectList />,
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
    path: '/login',
    element: <Login />,
    isProtected: false
  }
];

export default AppRoutes;
