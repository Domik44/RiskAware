import React, { Component } from 'react';
import { formatDate } from "../common/DateFormatter";
import { TabContent, TabPane, Nav, NavItem, NavLink, Alert, Row, Col } from 'reactstrap';
import PhaseAccordion from './PhaseAccordion';
import IProjectDetail, { RoleType } from './interfaces/IProjectDetail';
import AddPhaseModal from './modals/PhaseAddModal';
import AddProjectRoleModal from './modals/ProjectRoleAddModal';
import AddRiskModal from './modals/RiskAddModal';
import RiskDetail from './RiskDetail';
import IRiskDetail from './interfaces/IRiskDetail';
import CommentList from './lists/CommentList';
import InitialSetupModal from './modals/InitialSetupModal';
import Matrix from './matrix/Matrix';
import PhaseList from './lists/PhaseList';
import RiskList from './lists/RiskList';
import UsersOnProjectList from './lists/UsersOnProjectList';
import IDtFetchData from './interfaces/IDtFetchData';
import EditProjectModal from './modals/EditProjectModal';

interface IProjectDetailState {
  projectDetail: IProjectDetail | null;
  activeTab: string;
}

export class ProjectDetail extends Component<object, IProjectDetailState> {
  constructor(props: object) {
    super(props);
    this.state = {
      projectDetail: null,
      activeTab: 'detail',
    };
  }

  // Creating MutableRefObject because React.createRef is readonly
  phaseFetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };
  riskFetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };
  memberFetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };

  componentDidMount() {
    this.populateProjectDetail();
  }


  render() {
    const { projectDetail, activeTab } = this.state;
    const canAddWithoutPhase = projectDetail?.userRole === RoleType.ProjectManager || projectDetail?.userRole === RoleType.RiskManager;
    const canAddWithPhase = projectDetail?.userRole === RoleType.TeamMember && projectDetail?.assignedPhase !== null;

    const contents = !projectDetail
      ?
      (<Alert color="danger">
        Projekt nebyl nalezen.
      </Alert>)
      : (
        <div className="container">
          {projectDetail.detail.isBlank && (
            <InitialSetupModal projectDetail={projectDetail} />
          )}
          <div className="row">
            <div className="col-3">
              <h1 className="mb-3">{projectDetail.detail.title}</h1>
              <PhaseAccordion
                projectDetail={projectDetail}
                toggleTab={this.toggleTab}
                chooseRisk={this.chooseRisk}
              />
            </div>
            <div className="col-9">
              <Nav tabs className="flex-row-reverse mb-3">
                <NavItem>
                  <NavLink active={activeTab === 'matrix'} onClick={() => this.toggleTab('matrix')}> Matice </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink active={activeTab === 'risks'} onClick={() => this.toggleTab('risks')}> Rizika </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink active={activeTab === 'members'} onClick={() => this.toggleTab('members')}> Členové </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink active={activeTab === 'phases'} onClick={() => this.toggleTab('phases')}> Fáze </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink active={activeTab === 'detail'} onClick={() => this.toggleTab('detail')}> Detail </NavLink>
                </NavItem>
                <NavItem className={activeTab !== 'riskDetail' ? 'hidden' : ''}>
                  <NavLink active={activeTab === 'riskDetail'} onClick={() => this.toggleTab('riskDetail')} id="riskDetail"> Riziko </NavLink>
                </NavItem>
              </Nav>
              <TabContent activeTab={activeTab}>
                <RiskDetail
                  activeTab={activeTab}
                  projectDetail={projectDetail}
                />
                <TabPane tabId="detail">
                  <dl>
                    <Row className="mt-3">
                      <dt className="mb-2">Popis:</dt>
                      <dd>
                        <textarea readOnly className="form-control" value={projectDetail.detail.description} />
                      </dd>
                    </Row>
                    <Row className="mt-2">
                      <Col>
                        <dt>Od</dt>
                        <dd>{formatDate(projectDetail.detail.start)}</dd>
                      </Col>
                      <Col>
                        <dt>Do</dt>
                        <dd>{formatDate(projectDetail.detail.end)}</dd>
                      </Col>
                      <Col className="col-1">
                        {projectDetail.userRole === RoleType.ProjectManager && (
                          <EditProjectModal projectId={projectDetail.detail.id} reRender={this.reRender} />
                        )}
                      </Col>
                    </Row>
                    <hr className="mt-5"></hr>
                    {projectDetail.userRole !== RoleType.CommonUser && (
                      <Row className="mt-5">
                        <CommentList projId={projectDetail.detail.id} comments={projectDetail.detail.comments}></CommentList>
                      </Row>
                    )}
                  </dl>
                </TabPane>
                <TabPane tabId="phases">
                  <Row className="mb-3">
                    <Col>
                      <h5>Fáze projektu</h5>
                    </Col>
                    {projectDetail.userRole === RoleType.ProjectManager && (
                      <Col className="d-flex justify-content-end">
                        <AddPhaseModal projectDetail={projectDetail} reRender={this.reRender} fetchDataRef={this.phaseFetchDataRef} />
                      </Col>
                    )}
                  </Row>
                  <PhaseList projectId={projectDetail.detail.id} fetchDataRef={this.phaseFetchDataRef} reRender={this.reRender} projectDetail={projectDetail} />
                </TabPane>
                <TabPane tabId="risks">
                  <Row>
                    <Col>
                      <h5>Registr rizik</h5>
                    </Col>
                    <Col className="d-flex justify-content-end">
                      {(canAddWithoutPhase || canAddWithPhase) && (
                        <AddRiskModal projectDetail={projectDetail} reRender={this.reRender} fetchDataRef={this.riskFetchDataRef} />
                      )}
                    </Col>
                  </Row>
                  <RiskList
                    projectId={projectDetail.detail.id}
                    chooseRisk={this.chooseRisk}
                    fetchDataRef={this.riskFetchDataRef}
                    reRender={this.reRender}
                    projectDetail={projectDetail} />
                </TabPane>
                <TabPane tabId="members">
                  <Row className="mb-3">
                    <Col>
                      <h5>Členové projektu</h5>
                    </Col>
                    <Col className="d-flex justify-content-end">
                      {(projectDetail.userRole === RoleType.ProjectManager || projectDetail.userRole === RoleType.RiskManager) && (
                        <AddProjectRoleModal projectDetail={projectDetail} reRender={this.reRender} fetchDataRef={this.memberFetchDataRef} />
                      )}
                    </Col>
                  </Row>
                  <UsersOnProjectList projectId={projectDetail.detail.id} fetchDataRef={this.memberFetchDataRef} projectDetail={projectDetail} />
                </TabPane>
                <TabPane tabId="matrix">
                  <Row>
                    <Matrix detail={projectDetail} chooseRisk={this.chooseRisk}></Matrix>
                  </Row>
                </TabPane>
              </TabContent>
            </div>
          </div>
        </div>);

    return (
      <div>
        {contents}
      </div>
    );
  }

  async populateProjectDetail() {
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];

    const apiUrl = `/api/RiskProject/${id}`;
    try {
      const response = await fetch(apiUrl);
      const data: IProjectDetail = await response.json();
      this.setState({ projectDetail: data });
    } catch (error) {
      console.error('Error fetching project detail:', error);
    }
  }

  toggleTab = (tab: string) => {
    if (this.state.activeTab !== tab) {
      this.setState({ activeTab: tab });
    }
  }

  chooseRisk = async (id: number) => {
    try {
      const apiUrl = `/api/Risk/${id}`;
      const response = await fetch(apiUrl);
      const data: IRiskDetail = await response.json();
      this.setState(prevState => ({
        projectDetail: {
          ...prevState.projectDetail!,
          chosenRisk: data,
        },
      }));
      this.toggleTab('riskDetail');
    } catch (error) {
      console.error('Error fetching risk:', error);
    }
  };

  reRender = async () => {
    await this.populateProjectDetail();
  }
}

export default ProjectDetail;
