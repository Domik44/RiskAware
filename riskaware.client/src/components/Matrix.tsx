import { Component } from 'react';
import IProjectDetail from './interfaces/IProjectDetail';
import IRisks from './interfaces/IRisks';
import MatrixCell from './MatrixCell';

interface IMatrix {
  detail: IProjectDetail;
  chooseRisk: (id: number) => void;
}

interface IMatrixState {
  detail: IProjectDetail;
  scale: number;
  chooseRisk: (id: number) => void;
}

export class Matrix extends Component<IMatrix, IMatrixState> {
  constructor(props: IMatrix) {
    super(props);
    this.state = {
      detail: props.detail,
      scale: 5,
      chooseRisk: props.chooseRisk
    };
  }


  render() {
    const scale = this.state.scale;
    const risks = this.state.detail.risks


    let dataMatrix: IRisks[][][] = [];
    for (var i: number = 0; i < scale; i++) {
      dataMatrix[i] = [];
      for (var j: number = 0; j < scale; j++) {
        dataMatrix[i][j] = [];
      }
    }

    risks.forEach(risk => {
      dataMatrix[risk.impact - 1][risk.probability - 1].push(risk);
    });

    let cells = [];
    for (let imp: number = 0; imp < scale; imp++) {
      let row = [];
      for (let prob: number = 0; prob < scale; prob++) {
        const index = scale * imp + prob;
        const risks: IRisks[] = dataMatrix[imp][prob];
        row.push(<MatrixCell key={index} cellID={index} risks={risks} scale={scale} prob={prob + 1} impact={imp + 1} chooseRisk={this.state.chooseRisk}></MatrixCell>); //TODo not usre about key
      }
      cells.unshift(...row);
    }

    let contents;
    if (scale == 5) {
      contents = (
        <div className="container">
          <p>MATICE: 5x5</p>
          <div className="matrixTop">
            <div className="dopad">
              <div>Dopad</div>
            </div>
            <div>
              <div className="matrixCells">
                {cells}
              </div>
              <div className="pravdepodobnost">
                <div>Pravděpodobnost</div>
              </div>
            </div>
          </div>
        </div>);
    }
    else {
      contents = (
        <div className="container">
          <p>MATICE: 3x3 TODO</p>
          <div className="matrixGrid">
            <div>1</div>
            <div>2</div>
            <div>3</div>
            <div>4</div>
            <div>5</div>
            <div>6</div>
            <div>7</div>
            <div>8</div>
            <div>9</div>
          </div>
        </div>);
    }

    

    return (
      <div>
        {contents}
      </div>
    );
  }
}

export default Matrix;
