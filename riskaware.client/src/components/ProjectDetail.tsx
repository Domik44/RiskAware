import { Component } from 'react';
//import { Link } from 'react-router-dom';
import { formatDate } from "../helpers/DateFormatter";
//import { Button } from 'react-bootstrap';
import { TabContent, TabPane, Nav, NavItem, NavLink } from 'reactstrap';

interface IComments {
  id: number;
  text: string;
  created: Date;
  author: string;
}

interface IUser {
  //id: number;
  fullName: string;
  email: string;
}

interface IMembers {
  id: number;
  roleName: string;
  isReqApproved: boolean;
  user: IUser;
}
interface IDetail {
  title: string;
  description: string;
  start: Date;
  end: Date;
  comments: IComments[];
}

interface IProjectDetail {
  detail: IDetail;
  members: IMembers[];
}

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
        ? <p>Projekt nebyl nalezen.</p>
      :
      <div className="container">
        <h1>{projectDetail.detail.title}</h1>
        <div className="row">
          <div className="col-3">
            <p>"Panel"</p>
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
                ggdg
              </TabPane>
              <TabPane tabId="risks">
               fsafsa
              </TabPane>
              <TabPane tabId="members">
                <ul>
                  {projectDetail.members.map((member) => (
                    <li key={member.id}>
                      <p>{member.user.fullName}</p>
                      <p>{member.roleName}</p>
                      <p>{member.isReqApproved ? 'Approved' : 'Not approved'}</p>
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
    const response = await fetch(apiUrl); // TODO: handle when no project is found
    const data: IProjectDetail = await response.json();
    this.setState({ projectDetail: data });
  }

  toggleTab(tab: string) {
    if (this.state.activeTab !== tab) {
      this.setState({ activeTab: tab });
    }
  }

}
