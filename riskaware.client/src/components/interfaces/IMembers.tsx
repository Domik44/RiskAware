import IUser from "./IUser";

interface IMembers {
  id: number;
  roleName: string;
  isReqApproved: boolean;
  user: IUser;
}

export default IMembers;
