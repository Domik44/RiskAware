//import React from 'react';
import { Col, Row, TabPane } from 'reactstrap';
import IProjectDetail from './interfaces/IProjectDetail';
import { formatDate } from '../common/DateFormatter';

interface IRiskDetailProps {
  activeTab: string;
  projectDetail: IProjectDetail;
}

function RiskDetail(props: IRiskDetailProps) {
  const { activeTab, projectDetail } = props;

  return (
    <TabPane tabId="riskDetail" className={activeTab === 'riskDetail' ? 'active' : ''}>
      {projectDetail.chosenRisk ? (
        <dl>
          <Row>
            <h3>{projectDetail.chosenRisk.title}</h3>
          </Row>
          <dt>
            Popisek:
          </dt>
          <dd>
            <textarea readOnly value={projectDetail.chosenRisk.description} className="form-control" />
          </dd>
          <Row>
            <Col>
              <dt>
                Kategorie:
              </dt>
              <dd>
                {projectDetail.chosenRisk.riskCategoryName}
              </dd>
            </Col>
            <Col>
              <dt>
                Fáze:
              </dt>
              <dd>
                {projectDetail.chosenRisk.projectPhaseName}
              </dd>
            </Col>
          </Row>
          <Row>
            <Col>
              <dt>
                Pravděpodobnost:
              </dt>
              <dd>
                {projectDetail.chosenRisk.probability}
              </dd>
            </Col>
            <Col>
              <dt>
                Dopad:
              </dt>
              <dd>
                {projectDetail.chosenRisk.impact}
              </dd>
            </Col>
          </Row>
          <Row>
            <dt>
              Závažnost:
            </dt>
            <dd>
              {projectDetail.chosenRisk.severity}
            </dd>
          </Row>
          <Row>
            <dt>
              Hrozba:
            </dt>
            <dd>
              <textarea readOnly value={projectDetail.chosenRisk.threat} className="form-control" />
            </dd>
          </Row>
          <Row>
            <dt>
              Indikátory:
            </dt>
            <dd>
              <textarea readOnly value={projectDetail.chosenRisk.indicators} className="form-control" />
            </dd>
          </Row>
          <Row>
            <Col>
              <dt>
                Prevence:
              </dt>
              <dd>
                {projectDetail.chosenRisk.prevention}
              </dd>
            </Col>
            <Col>
              <dt>
                Stav:
              </dt>
              <dd>
                {projectDetail.chosenRisk.status}
              </dd>
            </Col>
          </Row>
          <Row>
            <Col>
              <dt>
                Prevence dokončena:
              </dt>
              <dd>
                {projectDetail.chosenRisk.preventionDone === new Date("0001-01-01") ?  formatDate(projectDetail.chosenRisk.preventionDone) : "Nevyplněno"}
              </dd>
            </Col>
            <Col>
              <dt>
                Poslední změna stavu:
              </dt>
              <dd>
                {formatDate(projectDetail.chosenRisk.statusLastModif)}
              </dd>
            </Col>
          </Row>
          <Row>
            <Col>
              <dt>
                Vytvořeno:
              </dt>
              <dd>
                {formatDate(projectDetail.chosenRisk.created)}
              </dd>
            </Col>
            <Col>
              <dt>
                Naposledy upraveno:
              </dt>
              <dd>
                {formatDate(projectDetail.chosenRisk.lastModif)}
              </dd>
            </Col>
          </Row>
          <Row>
            <Col>
              <dt>
                Konec:
              </dt>
              <dd>
                {projectDetail.chosenRisk.end === new Date("0001-01-01") ? formatDate(projectDetail.chosenRisk.end) : "Nevyplněno"}
              </dd>
            </Col>
            <Col>
              <dt>
                Riziko nastalo:
              </dt>
              <dd>
                {projectDetail.chosenRisk.riskEventOccured === new Date("0001-01-01") ? formatDate(projectDetail.chosenRisk.riskEventOccured) : "Nevyplněno"}
               </dd>
            </Col>
          </Row>
          <Row>
            <Col>
              <dt>
                Vytvořil:
              </dt>
              <dd>
                {projectDetail.chosenRisk.userFullName}
              </dd>
            </Col>
            <Col>
              <dt>
                Upravil:
              </dt>
              <dd>
                {projectDetail.chosenRisk.editedBy}
              </dd>
            </Col>
          </Row>
        </dl>
      ) : (
        <p>No risk selected.</p>
      )}
    </TabPane>
  );
}

export default RiskDetail;
