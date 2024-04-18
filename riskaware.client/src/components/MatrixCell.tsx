import { Component } from 'react';
import IRisks from './interfaces/IRisks';
import { PopoverBody, PopoverHeader, UncontrolledPopover } from 'reactstrap';

interface IMatrix {
  cellID: number
  risks: IRisks[]
  scale: number;
  prob: number;
  impact: number;
  chooseRisk: (id: number) => void;

}

interface IMatrixState {
  cellId: number
  risks: IRisks[]
  popoverOpen: boolean;
  scale: number;
  prob: number;
  impact: number;
  chooseRisk: (id: number) => void;
}

export class MatrixCell extends Component<IMatrix, IMatrixState> {
  constructor(props: IMatrix) {
    super(props);
    this.state = {
      cellId: props.cellID,
      risks: props.risks,
      scale: props.scale,
      prob: props.prob,
      impact: props.impact,
      popoverOpen: false,
      chooseRisk: props.chooseRisk,
    };
  }

  togglePopover = () => {
    this.setState(prevState => ({
      popoverOpen: !prevState.popoverOpen
    }));
  };

  //mixColor = (colorA: string, colorB: string, proportion: number): string => {
  //  // Convert hex color to RGB
  //  const hexToRgb = (hex: string): number[] => {
  //    const r = parseInt(hex.slice(1, 3), 16);
  //    const g = parseInt(hex.slice(3, 5), 16);
  //    const b = parseInt(hex.slice(5, 7), 16);
  //    return [r, g, b];
  //  };

  //  // Convert RGB to hex color
  //  const rgbToHex = (rgb: number[]): string => {
  //    return '#' + rgb.map(c => c.toString(16).padStart(2, '0')).join('');
  //  };

  //  const [redA, greenA, blueA] = hexToRgb(colorA);
  //  const [redB, greenB, blueB] = hexToRgb(colorB);

  //  // Calculate mixed color components
  //  const mixedRed = Math.round(redA * (1 - proportion) + redB * proportion);
  //  const mixedGreen = Math.round(greenA * (1 - proportion) + greenB * proportion);
  //  const mixedBlue = Math.round(blueA * (1 - proportion) + blueB * proportion);

  //  // Convert mixed color to hex
  //  return rgbToHex([mixedRed, mixedGreen, mixedBlue]);
  //}


  rgbToHex = (rgb: number[]): string => {
    // Convert each RGB component to hexadecimal
    const hexComponents = rgb.map(component => {
      // Ensure the component is within the valid range [0, 255]
      const componentHex = Math.min(255, Math.max(0, component));
      // Convert the component to hexadecimal and pad with zeros if necessary
      return componentHex.toString(16).padStart(2, '0');
    });

    // Concatenate the hexadecimal components and prepend '#' to form the hex color
    return '#' + hexComponents.join('');
  }

  render() {
    let IDstring = "cellID" + this.state.cellId;
    //const cellId = this.state.cellId;
    const prob = this.state.prob;
    const impact = this.state.impact;
    const risks = this.state.risks;
    const scale = this.state.scale;


    //background - color: #ccc;
    const a = [21, 41, 134];
    const b = [208, 117, 127];
    //const proportion = 1;
    const green = [26, 237, 7];
    const yellow = [255, 226, 5];
    const red = [237, 22, 7];
    let lowerColor = green;
    let higherColor = yellow;


    let threshold: number = prob + impact;
    let maxThreshold: number = scale + scale;

    if (threshold > scale) {
      threshold -= scale;
      maxThreshold -= scale;
      lowerColor = yellow;
      higherColor = red;
    }


    const ratio = threshold / maxThreshold;

    let c = [];
    for (let i = 0; i < 3; i++) {
      c.push(Math.round((lowerColor[i] * (1 - ratio) + higherColor[i] * ratio)));
    }
    let color = this.rgbToHex(c);


    let popover = <></>;
    let popoverText;
    if (risks.length > 0) {
      popoverText = [];
      for (let i: number = 0; i < risks.length; i++) {
        const risk = risks[i];
        popoverText.push(
          <div key={i}
            onClick={() => this.state.chooseRisk(risk.id)}
            className="clickable"
          >
            <p className="matrixPopoverRisk">{risk.title}</p>
          </div>);
      }
      popover = (
        <UncontrolledPopover
          placement="right"
          isOpen={this.state.popoverOpen}
          target={IDstring}
          trigger="legacy"
          toggle={this.togglePopover}
          style={{ minWidth: "150px", minHeight: "200px" }}
        >
          <PopoverHeader>
            Priorita: {impact * prob}
          </PopoverHeader>
          <PopoverBody>
            {popoverText}
          </PopoverBody>
        </UncontrolledPopover>);
    }

    let cnt = "";
    if (risks.length > 0)
      cnt = risks.length.toString();
    let contents = (
      <div className="matrixCell"
        id={IDstring}
        onClick={this.togglePopover}
        style={{ cursor: 'pointer', backgroundColor: color }}>
        <p className="riskCnt">{cnt}</p>
          {popover}
      </div>
    );


    //console.log("I:", impact, "P:", prob, this.state.risks);
    //for (let i: number = 0; i < risks.length; i++) {
    //  if (impact == risks[i].impact && prob == risks[i].probability)
    //    console.log("all good");
    //  else
    //    console.log("FUCK");
    //}


    return (
      <div>
        {contents}
      </div>
    );
  }
}

export default MatrixCell;
