import IComments from "./IComments";

/**
 * This interface is used to match RiskProjectDetailDto.
 * Its purpose is to define attributes for Detail tab.
 * */
interface IDetail {
  id: number;
  title: string;
  description: string;
  start: Date;
  end: Date;
  comments: IComments[];
  isBlank: boolean;
  // TODO -> isBlank
}

export default IDetail;
