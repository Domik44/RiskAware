import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import ProjectsList from './ProjectsList';
import CreateProjectModal from '../CreateProjectModal';
export class MyProjectsList extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Vlastní projekty</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <CreateProjectModal />
          </Col>
        </Row>
        <ProjectsList fetchUrl={'/api/RiskProject/UserRiskProjects'} />
      </div>
    );
  }
}

export default MyProjectsList;
