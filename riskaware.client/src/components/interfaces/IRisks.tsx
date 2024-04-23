import ISimplePhase from "./ISimplePhase";

interface IRisks {
  id: number;
  title: string;
  categoryName: string;
  severity: number;
  probability: number;
  impact: number;
  state: string;
  projectPhase: ISimplePhase;
}

export default IRisks;
