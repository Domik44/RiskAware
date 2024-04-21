import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import CreateProjectModal from '../modals/CreateProjectModal';
import IFetchData from '../../common/IFetchData';
import ProjectsList from './ProjectsList';

export class MyProjectsList extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  fetchDataRef: React.MutableRefObject<IFetchData | null> = { current: null };

  render() {
    return (
      <>
        <Row className="mb-3">
          <Col>
            <h4>Vlastní projekty</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <CreateProjectModal fetchDataRef={this.fetchDataRef} />
          </Col>
        </Row>
        <ProjectsList fetchUrl={'/api/RiskProject/UserRiskProjects'} fetchDataRef={this.fetchDataRef} />
      </>
    );
  }
}

export default MyProjectsList;
