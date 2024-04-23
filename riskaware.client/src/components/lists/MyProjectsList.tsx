import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import CreateProjectModal from '../modals/CreateProjectModal';
import IDtFetchData from '../interfaces/IDtFetchData';
import ProjectsList from './ProjectsList';
import AuthContext, { AuthContextType } from '../../auth/AuthContext';

export class MyProjectsList extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };

  render() {
    return (
      <AuthContext.Consumer>
        {(authContext: AuthContextType | null) => (
          <>
            <Row className="mb-3">
              <Col>
                <h4>Vlastní projekty</h4>
              </Col>
              <Col className="d-flex justify-content-end">
                {authContext?.isAdmin && <CreateProjectModal fetchDataRef={this.fetchDataRef} />}
              </Col>
            </Row>
            <ProjectsList fetchUrl={'/api/RiskProject/UserRiskProjects'} fetchDataRef={this.fetchDataRef} />
          </>
        )}
      </AuthContext.Consumer>
    );
  }
}

export default MyProjectsList;
