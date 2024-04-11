import { Component } from 'react';
//import { Link } from 'react-router-dom';
//import { formatDate } from "../helpers/DateFormatter";
//import { Button } from 'react-bootstrap';
import { Button } from 'reactstrap';

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

export class ProjectDetail extends Component<object, { projectDetail: IProjectDetail | undefined }> {
  constructor(props: object) {
    super(props);
    this.state = {
      projectDetail: undefined
    };
  }

  componentDidMount() {
    this.populateProjectDetail();
  }

  render() {
    const { projectDetail } = this.state;

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
            {/*<p>"Navtaby"</p>*/}
            {/*<p>{projectDetail.members.pop()?.user.email}</p>*/}
            {/*<p>{projectDetail.detail.description}</p>*/}
            <ul className="nav nav-pills" id="myTab" role="tablist">
              <li className="nav-item" role="presentation">
                <Button className="nav-link active" id="detail-tab" data-bs-toggle="tab" data-bs-target="#detail" role="tab" aria-controls="detail" aria-selected="true">Detail</Button>
              </li>
              <li className="nav-item" role="presentation">
                <Button className="nav-link" id="phases-tab" data-bs-toggle="tab" data-bs-target="#phases" role="tab" aria-controls="phases" aria-selected="false">Fáze</Button>
              </li>
              <li className="nav-item" role="presentation">
                <Button className="nav-link" id="risks-tab" data-bs-toggle="tab" data-bs-target="#risks" role="tab" aria-controls="risks" aria-selected="false">Rizika</Button>
              </li>
              <li className="nav-item" role="presentation">
                <Button className="nav-link" id="members-tab" data-bs-toggle="tab" data-bs-target="#members" role="tab" aria-controls="members" aria-selected="false">Členové</Button>
              </li>
            </ul>
            <div className="tab-content" id="myTabContent">
              <div className="tab-pane fade show active" id="detail" role="tabpanel" aria-labelledby="detail-tab">Popisek</div>
              <div className="tab-pane fade" id="phases" role="tabpanel" aria-labelledby="phases-tab">Faze</div>
              <div className="tab-pane fade" id="risks" role="tabpanel" aria-labelledby="risks-tab">rizika</div>
              <div className="tab-pane fade" id="members" role="tabpanel" aria-labelledby="members-tab">clenove</div>
            </div>
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
}
