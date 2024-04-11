import { Component } from 'react';
import ProjectList, { IProject } from './ProjectList';

export class MyProjectList extends Component < object, { projects: IProject[] | undefined } > {
  constructor(props: object) {
    super(props);
    this.state = {
      projects: undefined
    };
  }

  componentDidMount() {
    this.populateProjectListData();
  }

  async populateProjectListData() {
    const response = await fetch("/api/RiskProject/UserRiskProjects");
    //const response = await fetch("/api/RiskProject/UserRiskProjects", { credentials: 'include'});   // todo delete previous version
    const data = await response.json();
    this.setState({ projects: data });
  }

  render() {
    return (
      <div>
        <h2 id="projectListLabel">Vlastní projekty</h2>
        {this.state.projects ? <ProjectList projects={this.state.projects} />
          : <p>Loading... Please refresh once the ASP.NET backend has started.</p>}
      </div>
    );
  }
}

export default MyProjectList;
