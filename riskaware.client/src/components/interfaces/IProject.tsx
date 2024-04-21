interface IProject {
  id: number;
  title: string;
  start: Date;
  end: Date;
  numOfMembers: string;
  projectManagerName: string;
  isValid: boolean;
}

export default IProject;
