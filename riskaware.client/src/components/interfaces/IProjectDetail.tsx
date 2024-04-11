import IDetail from "./IDetail";
import IMembers from "./IMembers";
import IPhases from "./IPhases";
import IRisks from "./IRisks";

interface IProjectDetail {
  detail: IDetail;
  members: IMembers[];
  risks: IRisks[];
  phases: IPhases[];
}

export default IProjectDetail;
