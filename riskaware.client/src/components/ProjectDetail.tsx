import { Component } from 'react';
import { formatDate } from "../helpers/DateFormatter";
import { TabContent, TabPane, Nav, NavItem, NavLink, Alert, Row, Col} from 'reactstrap';
import PhaseAccordion from './PhaseAccordion';
import IProjectDetail, { RoleType } from './interfaces/IProjectDetail';
import AddPhaseModal from './AddPhaseModal';
import AddProjectRoleModal from './AddProjectRoleModal';
import AddRiskModal from './AddRiskModal';
import RiskDetail from './RiskDetail'; // Import the RiskDetail component
import IRiskDetail from './interfaces/IRiskDetail';
import CommentList from './CommentList';
import InitialSetupModal from './InitialSetupModal';
import Matrix from './Matrix';

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

  componentDidMount() {
    this.populateProjectDetail();
  }

  render() {
    const { projectDetail, activeTab } = this.state;

    const contents = !projectDetail
      ?
      (<Alert color="danger">
        Projekt nebyl nalezen.
      </Alert>)
      :(
      <div className="container">
        {projectDetail.detail.isBlank && (
          <InitialSetupModal projectDetail={projectDetail} />
        )}
        <h1>{projectDetail.detail.title}</h1>
        <div className="row">
            <div className="col-3">
            <PhaseAccordion
              projectDetail={projectDetail}
              toggleTab={this.toggleTab}
              chooseRisk={this.chooseRisk}
            />
          </div>
            <div className="col-9">
              <Nav tabs className="flex-row-reverse">
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
                    </Row>
                    {projectDetail.userRole !== RoleType.CommonUser && (
                      <Row className="mt-5">
                        <CommentList projId={projectDetail.detail.id} comments={projectDetail.detail.comments }></CommentList>
                      </Row>
                    )}
                </dl>
              </TabPane>
                <TabPane tabId="phases">
                  {projectDetail.userRole === RoleType.ProjectManager && ( 
                    <AddPhaseModal projectDetail={projectDetail} />
                  )}
                <ul>
                  {projectDetail.phases.map((phase) => (
                    <li key={phase.id}>
                      <p>{phase.order}</p>
                      <p>{phase.name}</p>
                      <p>{formatDate(phase.start)} - {formatDate(phase.end)}</p>
                    </li>
                  ))}
                </ul>
              </TabPane>
                <TabPane tabId="risks">
                  
                  <AddRiskModal projectDetail={projectDetail} />
                <ul>
                  {projectDetail.risks.map((risk) => (
                    <li key={risk.id}>
                      <p>{risk.title}</p>
                      <p>{risk.categoryName}</p>
                      <p>{risk.severity}</p>
                      <p>{risk.probability}</p>
                      <p>{risk.impact}</p>
                      <p>{risk.state}</p>
                    </li>
                  ))}
                </ul>
              </TabPane>
                <TabPane tabId="members">
                  {projectDetail.userRole === RoleType.ProjectManager && ( 
                    <AddProjectRoleModal projectDetail={projectDetail} />
                  )}
                <ul>
                  {projectDetail.members.map((member) => (
                    <li key={member.id}>
                      <p>{member.user.fullName}</p>
                      <p>{member.roleName}</p>
                      <p>{member.isReqApproved ? 'Approved' : 'Not approved'}</p>
                      <p>{member.projectPhaseName}</p>
                    </li>
                  ))}
                </ul>
              </TabPane>
              <TabPane tabId="matrix">
                <Matrix detail={projectDetail}></Matrix>
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
    console.log('populateProjectDetail');
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

    //if (urlSplitted.length === 5) {
    //  const riskId = urlSplitted[4];
    //  // TODO -> mby delete -> would have to check if id in url is of risk from this project otherwise it would load risk from other projects as well
    //  // TODO -> would have to manually change url when clicking on other nav tabs
    //  this.chooseRisk(parseInt(riskId));
    //}
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
}

export default ProjectDetail;
