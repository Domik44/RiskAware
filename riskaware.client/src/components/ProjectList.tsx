import { Link } from 'react-router-dom';
import { formatDate } from "../helpers/DateFormatter";

export interface IProject {
  id: number;
  title: string;
  start: Date;
  end: Date;
  numOfMembers: string;
  projectManagerName: string;
}

const ProjectList: React.FC<{ projects: IProject[] }> = ({ projects }) => {
  return (
    <table className="table small table-bordered" aria-labelledby="projectListLabel">
      <thead>
        <tr>
          <th>Id</th>
          <th>Název</th>
          <th>Projektový manažer</th>
          <th>Začátek</th>
          <th>Konec</th>
          <th>Počet členů</th>
          <th>Akce</th>
        </tr>
      </thead>
      <tbody>
        {projects.map(project =>
          <tr key={project.id}>
            <td>{project.id}</td>
            <td>{project.title}</td>
            <td>{project.projectManagerName}</td>
            <td>{formatDate(project.start)}</td>
            <td>{formatDate(project.end)}</td>
            <td>{project.numOfMembers}</td>
            <td>
              <Link
                className="btn btn-primary"
                title="Zobrazit projekt"
                to={`/project/${project.id}`}>
                <i className="bi bi-eye"></i>
                {/*<i className="bi bi-pencil-square"></i>*/}
              </Link>
            </td>
          </tr>
        )}
      </tbody>
    </table>
  );
}

export default ProjectList;
