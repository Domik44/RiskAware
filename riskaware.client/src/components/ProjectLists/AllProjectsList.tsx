import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import ProjectsList from './ProjectsList';
import CreateProjectModal from '../CreateProjectModal';
export class AllProjectsList extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  // todo show CreateProjectModal only to admin
  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Všechny projekty</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <CreateProjectModal />
          </Col>
        </Row>
        <ProjectsList fetchUrl={'/api/RiskProjects'} />
      </div>
    );
  }
}

export default AllProjectsList;
