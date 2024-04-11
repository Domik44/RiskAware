import { Component } from 'react';
import ProjectList, { IProject } from './ProjectList';

export class AllProjectList extends Component<object, { projects: IProject[] | undefined }> {
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
    const response = await fetch("/api/RiskProjects");
    const data = await response.json();
    this.setState({ projects: data });
  }

  render() {
    return (
      <div>
        <h2 id="projectListLabel">Všechny projekty</h2>
        {this.state.projects ? <ProjectList projects={this.state.projects} />
          : <p>Loading... Please refresh once the ASP.NET backend has started.</p>}
      </div>
    );
  }
}

export default AllProjectList;
