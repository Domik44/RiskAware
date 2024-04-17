import { Component } from 'react';
import IProjectDetail from './interfaces/IProjectDetail';
import IRisks from './interfaces/IRisks';
import { Button, PopoverBody, PopoverHeader, UncontrolledPopover } from 'reactstrap';

interface IMatrix {
  detail: IProjectDetail;
}

interface IMatrixState {
  detail: IProjectDetail;
  scale: number;
}

export class Matrix extends Component<IMatrix, IMatrixState> {
  constructor(props: IMatrix) {
    super(props);
    this.state = {
      detail: props.detail,
      scale: 5
    };
  }


  render() {
    //const { comments } = this.state;
    //const displayComments = comments.slice(0, this.state.maxDisplayCnt);
    //const hideLoadButtons = comments.length <= this.state.maxDisplayCnt;
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
      dataMatrix[risk.probability - 1][risk.impact - 1].push(risk);
    });
    // print
    console.log('dataMatrix');
    for (var i: number = 0; i < scale; i++) {
      for (var j: number = 0; j < scale; j++) {
        if (dataMatrix[i][j].length > 0)
          console.log(i, j, dataMatrix[i][j]);
      }
    }

    let contents;
    if (scale == 5) {
      contents = (
        <div className="container">
          <div className="Matrix">
            <p>MATICE: 5x5</p>
            <div className = "row">
              <div>1</div>
              <div>
                <Button
                  id="PopoverLegacy"
                  type="button"
                >
                  Launch Popover (Legacy)
                </Button>
                <UncontrolledPopover
                  placement="bottom"
                  target="PopoverLegacy"
                  trigger="legacy"
                >
                  <PopoverHeader>
                    Legacy Trigger
                  </PopoverHeader>
                  <PopoverBody>
                    Legacy is a reactstrap special trigger value (outside of bootstrap‘s spec/standard). Before reactstrap correctly supported click and focus, it had a hybrid which was very useful and has been brought back as trigger=“legacy“. One advantage of the legacy trigger is that it allows the popover text to be selected while also closing when clicking outside the triggering element and popover itself.
                  </PopoverBody>
                </UncontrolledPopover>
              </div>
              <div>2</div>
              <div>3</div>
              <div>4</div>
              <div>5</div>
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
