interface IRiskDetail {
  id: number;
  title: string;
  description: string;
  categoryName: string;
  probability: number;
  impact: number;
  severity: number;
  state: string;
  threat: string;
  indicators: string;
  prevention: string;
  //phaseName: string; // TODO -> depends on backend dto
  // TODO -> dates -> depends on backend dto
}

export default IRiskDetail;
