﻿import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import IProject from '../interfaces/IProject';
import ProjectsList from './ProjectsList';
import CreateProjectModal from '../CreateProjectModal';
export class AllProjectsList extends Component<object, { projects: IProject[] | undefined }> {
  constructor(props: object) {
    super(props);
    this.state = {
      projects: undefined
    };
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Všechny projekty</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <CreateProjectModal>
            </CreateProjectModal>
          </Col>
        </Row>
        <ProjectsList fetchUrl={ '/api/RiskProjects' } />
      </div>
    );

  }
}

export default AllProjectsList;