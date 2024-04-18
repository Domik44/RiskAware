interface IRiskDetail {
  id: number;
  title: string;
  description: string;
  probability: number;
  severity: number;
  impact: number;
  threat: string;
  indicators: string;
  prevention: string;
  status: string;
  preventionDone: Date;
  riskEventOccured: Date;
  end: Date;
  lastModif: Date;
  created: Date;
  statusLastModif: Date;
  projectPhaseName: string;
  riskCategoryName: string;
  isValid: boolean;
  isApproved: boolean;
  userFullName: string;
}

export default IRiskDetail;
