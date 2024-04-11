import IRiskUnderPhase from "./IRiskUnderPhase";

interface IPhases {
  id: number;
  order: number;
  name: string;
  start: Date;
  end: Date;
  risks: IRiskUnderPhase[];
}

export default IPhases;
