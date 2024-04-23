import { Component } from 'react';
import IProjectDetail from '../interfaces/IProjectDetail';
import IRisks from '../interfaces/IRisks';
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
      scale: props.detail.detail.scale,
      chooseRisk: props.chooseRisk
    };
  }


  render() {
    const scale = this.state.scale;
    const risks = this.state.detail.risks;


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
    let scaleName = "matrixScale5";
    let scaleNameH = "matrixScale5H";
    if (this.state.scale == 3) {
      scaleName = "matrixScale3";
      scaleNameH = "matrixScale3H";

    }

    let gridString: string = "repeat(" + scale + ", minmax(90px, 1fr))"
    let contents;
    contents = (
      <div className="container">
        <div>
          <h2>Priorita = Dopad * Pravděpodobnost</h2>
        </div>
        <div className="matrixTop">
          <div className="dopad">
            <div>
              <div >Dopad</div>
              <div className={scaleNameH}>
                <div>1</div>
                <div>{this.state.scale}</div>
              </div>
            </div>
          </div>
          <div>
            <div className="matrixCells" style={{ gridTemplateColumns: gridString }}>
              {cells}
            </div>
            <div className="pravdepodobnost">
              <div>
                <div className={scaleName }>
                  <div>1</div>
                  <div>{this.state.scale}</div>
                </div>
                <div>Pravděpodobnost</div>
              </div>
            </div>
          </div>
        </div>
      </div>);



    return (
      <div>
        {contents}
      </div>
    );
  }
}

export default Matrix;
