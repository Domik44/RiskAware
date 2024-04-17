interface IRisks {
  id: number;
  title: string;
  categoryName: string;
  severity: number;
  probability: number;
  impact: number;
  state: string;
}

export default IRisks;
