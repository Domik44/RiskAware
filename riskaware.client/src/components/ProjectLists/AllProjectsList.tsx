import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import ProjectsList from './ProjectsList';
import CreateProjectModal from '../CreateProjectModal';
import IFetchData from '../../common/IFetchData';

export class AllProjectsList extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  fetchDataRef: React.MutableRefObject<IFetchData | null> = { current: null };

  // todo show CreateProjectModal only to admin
  render() {
    return (
      <>
        <Row className="mb-3">
          <Col>
            <h4>Všechny projekty</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <CreateProjectModal fetchDataRef={this.fetchDataRef} />
          </Col>
        </Row>
        <ProjectsList fetchUrl={'/api/RiskProjects'} fetchDataRef={this.fetchDataRef} />
      </>
    );
  }
}

export default AllProjectsList;
