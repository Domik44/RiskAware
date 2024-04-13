import { Component } from 'react';
//import { Link } from 'react-router-dom';
import { formatDate } from "../helpers/DateFormatter";
//import { Button } from 'react-bootstrap';
import { TabContent, TabPane, Nav, NavItem, NavLink, Alert } from 'reactstrap';
import PhaseAccordition from './PhaseAccordion';
import IProjectDetail from './interfaces/IProjectDetail';

export class ProjectDetail extends Component<object, { projectDetail: IProjectDetail | undefined, activeTab: string }> {
  constructor(props: object) {
    super(props);
    this.state = {
      projectDetail: undefined,
      activeTab: 'detail'
    };
  }

  componentDidMount() {
    this.populateProjectDetail();
  }

  render() {
    const { projectDetail, activeTab } = this.state;

    const contents = projectDetail === undefined
      ?
      <Alert color="danger">
        Projekt nebyl nalezen.
      </Alert>
      :
      <div className="container">
        <h1>{projectDetail.detail.title}</h1>
        <div className="row">
          <div className="col-3">
            {/*Place accordion here*/}
            <PhaseAccordition projectDetail={projectDetail} />
          </div>
          <div className="col-9">
            <Nav pills>
              <NavItem>
                <NavLink active={activeTab === 'detail'} onClick={() => this.toggleTab('detail')}> Detail </NavLink>
              </NavItem>
              <NavItem>
                <NavLink active={activeTab === 'phases'} onClick={() => this.toggleTab('phases')}> Fáze </NavLink>
              </NavItem>
              <NavItem>
                <NavLink active={activeTab === 'risks'} onClick={() => this.toggleTab('risks')}> Rizika </NavLink>
              </NavItem>
              <NavItem>
                <NavLink active={activeTab === 'members'} onClick={() => this.toggleTab('members')}> Členové </NavLink>
              </NavItem>
            </Nav>
            <TabContent activeTab={activeTab}>
              <TabPane tabId="detail">
                <p>{projectDetail.detail.title}</p>
                <p>{projectDetail.detail.description}</p>
                <p>{formatDate(projectDetail.detail.start)} - {formatDate(projectDetail.detail.end)}</p>
                <ul>
                  {projectDetail.detail.comments.map((comment) => (
                    <li key={comment.id}>
                      <p>{comment.text}</p>
                      <p>Created: {formatDate(comment.created)}</p>
                      <p>Author: {comment.author}</p>
                    </li>
                  ))}
                </ul>
              </TabPane>
              <TabPane tabId="phases">
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
                <ul>
                  {projectDetail.risks.map((risk) => ( 
                    <li key={risk.id}>
                      <p>{risk.title}</p>
                      <p>{risk.categoryName}</p>
                      <p>{risk.severity}</p>
                      <p>{risk.state}</p>
                    </li>
                  ))}
                </ul>
              </TabPane>
              <TabPane tabId="members">
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
            </TabContent>
          </div>
        </div>
      </div>;

    return (
      <div>
        {contents}
      </div>);
  }

  async populateProjectDetail() {
    const id = window.location.pathname.split('/').pop();
    const apiUrl = `/api/RiskProject/${id}`;
    const response = await fetch(apiUrl);
    const data: IProjectDetail = await response.json();
    this.setState({ projectDetail: data });
  }

  toggleTab(tab: string) {
    if (this.state.activeTab !== tab) {
      this.setState({ activeTab: tab });
    }
  }

}
