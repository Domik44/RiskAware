import IComments from "./IComments";

interface IDetail {
  title: string;
  description: string;
  start: Date;
  end: Date;
  comments: IComments[];
}

export default IDetail;
