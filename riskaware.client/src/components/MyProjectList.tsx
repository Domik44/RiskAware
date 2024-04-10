import { Component } from 'react';
import { Link } from 'react-router-dom';
import { API_URL } from "../auth/constants";

interface IMyProjectList {
  id: number;
  title: string;
  start: Date;
  end: Date;
  numOfMembers: string;
  projectManagerName: string;
}

export class MyProjectList extends Component<object, { projects: IMyProjectList[] | undefined }> {
  constructor(props: object) {
    super(props);
    this.state = {
      projects: undefined
    };
  }

  componentDidMount() {
    this.populateProjectListData();
  }

  render() {
    const { projects } = this.state;
    const contents = projects === undefined
      ? <p>Loading... Please refresh once the ASP.NET backend has started.</p>
      : <table className="table small table-bordered" aria-labelledby="myProjectListLabel">
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
              <td>{project.start.toString()}</td>   {/* todo .toLocaleDateString('cs-CZ')*/}
              <td>{project.end.toString()}</td>
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
      </table>;

    return (
      <div>
        <h2 id="myProjectListLabel">Vlastní projekt</h2>
        {contents}
      </div>
    );
  }

  async populateProjectListData() {
    // todo try to fetch without using ${API_URL} as in weatherforecast
    const response = await fetch(`${API_URL}/api/RiskProject/UserRiskProjects`,
      {
        credentials: 'include',
      });
    const data: IMyProjectList[] = await response.json();
    this.setState({ projects: data });
  }
}
