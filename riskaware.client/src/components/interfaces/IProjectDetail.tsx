import IDetail from "./IDetail";
import IMembers from "./IMembers";
import IPhases from "./IPhases";
import IRiskDetail from "./IRiskDetail";
import IRisks from "./IRisks";

export enum RoleType {
  ProjectManager = 0,
  RiskManager = 1,
  TeamMember = 2,
  ExternalMember = 3,
  CommonUser = 4 // This means he user has no role assigned to the project
 }

interface IProjectDetail {
  detail: IDetail;
  members: IMembers[];
  risks: IRisks[];
  phases: IPhases[];
  chosenRisk: IRiskDetail;
  userRole: RoleType;
  //isAdmin: boolean; // TODO -> example
}

export default IProjectDetail;
