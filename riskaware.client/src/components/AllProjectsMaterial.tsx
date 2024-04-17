import { Component } from 'react';
import ProjectTableMaterial from './ProjectTable/ProjectTableMaterial';
import IProject from './interfaces/IProject';
import { Col, Row } from 'reactstrap';
import CreateProjectModal from './CreateProjectModal';
export class AllProjectsMaterial extends Component<object, { projects: IProject[] | undefined }> {
  constructor(props: object) {
    super(props);
    this.state = {
      projects: undefined
    };
  }

  render() {
    return (
      <div>
        {/*<h2>Material table</h2>*/}
        <Row>
          <Col>
            <h4>Všechny projekty - MT</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <CreateProjectModal>
            </CreateProjectModal>
          </Col>
        </Row>
        <ProjectTableMaterial />
      </div>
    );

  }
}

export default AllProjectsMaterial;
