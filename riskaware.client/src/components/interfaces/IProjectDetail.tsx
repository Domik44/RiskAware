import IDetail from "./IDetail";
import IMembers from "./IMembers";
import IPhases from "./IPhases";
import IRiskDetail from "./IRiskDetail";
import IRisks from "./IRisks";

interface IProjectDetail {
  detail: IDetail;
  members: IMembers[];
  risks: IRisks[];
  phases: IPhases[];
  chosenRisk: IRiskDetail;
  //isAdmin: boolean; // TODO -> example
}

export default IProjectDetail;
