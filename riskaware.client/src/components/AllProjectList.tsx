import { Component } from 'react';
import ProjectTable from './ProjectTable/ProjectTable';
import { QueryClientProvider, QueryClient } from 'react-query';
import IProject from './interfaces/IProject';

const queryClient = new QueryClient();
export class AllProjectList extends Component<object, { projects: IProject[] | undefined }> {
  constructor(props: object) {
    super(props);
    this.state = {
      projects: undefined
    };
  }

  //componentDidMount() {
  //  this.populateProjectListData();
  //}

  //async populateProjectListData() {
  //  const response = await fetch("/api/RiskProjects");
  //  const data = await response.json();
  //  this.setState({ projects: data });
  //}

  render() {
    return (
      <div>
        {/*<h2 id="projectListLabel">Všechny projekty</h2>*/}
        <QueryClientProvider client={queryClient}>
          <ProjectTable />
        </QueryClientProvider>
        {/*{this.state.projects ?*/}
        {/*  <QueryClientProvider client={queryClient}>*/}
        {/*    <ProjectList projects={this.state.projects} />*/}
        {/*  </QueryClientProvider>*/}
        {/*  : <p>Loading... Please refresh once the ASP.NET backend has started.</p>}*/}
      </div>
    );
  }
}

export default AllProjectList;
