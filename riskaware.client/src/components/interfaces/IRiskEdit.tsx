import { RoleType } from "./IProjectDetail";
import IRiskCategory from "./IRiskCategory";

interface IRiskEdit {
  id: number;
  title: string;
  description: string;
  probability: number;
  impact: number;
  threat: string;
  indicators: string;
  prevention: string;
  status: string;
  preventionDone: Date;
  riskEventOccured: Date;
  end: Date;
  projectPhaseId: number;
  riskCategory: IRiskCategory;
  userRoleType: RoleType
}

export default IRiskEdit;
