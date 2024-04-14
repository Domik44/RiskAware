//import React from 'react';
import { TabPane } from 'reactstrap';
import IProjectDetail from './interfaces/IProjectDetail';
//import IRiskDetail from './interfaces/IRiskDetail';

interface IRiskDetailProps {
  activeTab: string;
  projectDetail: IProjectDetail;
}

function RiskDetail(props: IRiskDetailProps) {
  const { activeTab, projectDetail } = props;

  return (
    <TabPane tabId="riskDetail" className={activeTab === 'riskDetail' ? 'active' : ''}>
      {projectDetail.chosenRisk ? (
        <div>
          <p>Title: {projectDetail.chosenRisk.title}</p>
          <p>Category: {projectDetail.chosenRisk.categoryName}</p>
          <p>Severity: {projectDetail.chosenRisk.severity}</p>
        </div>
      ) : (
        <p>No risk selected.</p>
      )}
    </TabPane>
  );
}

export default RiskDetail;
