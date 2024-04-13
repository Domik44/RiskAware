import IComments from "./IComments";

/**
 * This interface is used to match RiskProjectDeatailDto.
 * Its purpose is to define attributes for Detail tab.
 * */
interface IDetail {
  id: number;
  title: string;
  description: string;
  start: Date;
  end: Date;
  comments: IComments[];
}

export default IDetail;
