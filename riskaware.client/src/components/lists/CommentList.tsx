import { ChangeEvent, Component, FormEvent } from 'react';
import { formatDate } from "../../common/DateFormatter";
import { Form, Button, ButtonGroup, Row, Col } from 'reactstrap';
import CommentCard from '../CommentCard';
import IComments from '../interfaces/IComments';
import SendIcon from '@mui/icons-material/Send';

interface ICommentList {
  projId: number;
  comments: IComments[];
}

interface ICommentListState {
  comments: IComments[]; // Include comments property in state interface
  comment: string;
  projId: number;
  maxDisplayCnt: number;
  submitButtonDisabled: boolean;
}

export class CommentList extends Component<ICommentList, ICommentListState> {
  constructor(props: ICommentList) {
    super(props);
    this.state = {
      comments: props.comments,
      comment: '',
      projId: props.projId,
      maxDisplayCnt: 5,
      submitButtonDisabled: true,
    };
  }

  handleCommentSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const comment = this.state.comment;
    const id = this.state.projId;

    //send comment
    try {
      const apiUrl = `/api/RiskProject/AddComment?riskProjectId=${id}&text=${comment}`;
      const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      if (response.ok) {
        console.log('Comment added successfully!');
        const textarea = document.getElementById('commentTextArea') as HTMLTextAreaElement;
        if (textarea) {
          textarea.value = ''; // Clear textarea value
        }
        this.setState({
          submitButtonDisabled: true
        })
      } else {
        console.error('Failed to add comment');
      }
    } catch (error) {
      console.error('Error:', error);
    }

    // refresh
    this.refresh();
  };

  refresh = async () => {
    const id = this.state.projId;

    try {
      const apiUrl = `/api/RiskProject/${id}/GetComments`;
      const response = await fetch(apiUrl);
      const comments: IComments[] = await response.json();
      this.setState(() => ({
        comments: comments
      }));
    } catch (error) {
      console.error('Error fetching comments:', error);
    }
  };

  setButtonEnabled = (enable: boolean) => {
    const button = document.getElementById('submitCommentB') as HTMLButtonElement;
    button.disabled = !enable;
  }


  handleCommentChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
    const str = event.target.value;
    this.setState({ comment: str });

    if (str.length == 0) {
      this.setState({
        submitButtonDisabled: true
      })
    }
    else {
      this.setState({
        submitButtonDisabled: false
      })
    }
  };

  loadMore = (event: React.MouseEvent<HTMLButtonElement>)=> {
    console.log(event)
    const newThreshold = this.state.maxDisplayCnt + 5;
    this.setState({
      maxDisplayCnt: newThreshold
    })
  };

  loadAll = (event: React.MouseEvent<HTMLButtonElement>) => {
    console.log(event)
    this.setState({
      maxDisplayCnt: Number.MAX_SAFE_INTEGER
    })
  };

  render() {
    const { comments } = this.state;
    const displayComments = comments.slice(0, this.state.maxDisplayCnt);
    const hideLoadButtons = comments.length <= this.state.maxDisplayCnt;

    const contents = (
      <div className="container p-0">
        <h5>Komentáře:</h5>
        <Form id="addCommentForm" onSubmit={this.handleCommentSubmit} className="mb-3">
            <Row>
              <Col className="col-10">
                <div className="addComment">
                      <textarea
                        id="commentTextArea"  
                        className="form-control"
                        placeholder="Přidejte komentář..."

                      onChange={this.handleCommentChange}></textarea>
                </div>
              </Col>
              <Col className="col-2 commentButtonContainer">
                <div >
                    <Button
                      id="submitCommentB"
                      type="submit"
                      color="primary"
                      disabled={this.state.submitButtonDisabled}>
                      <SendIcon></SendIcon>
                    </Button>
                </div>
              </Col>
            </Row>
        </Form>
        {displayComments.map((comment) => (
          <CommentCard
          key={comment.id}
          username={comment.author}
          date={formatDate(comment.created)}
          text={comment.text}>
        </CommentCard>
        ))}
        <div className="commentButtonContainer" hidden={hideLoadButtons}>
          <ButtonGroup>
            <Button color="primary" onClick={this.loadMore}>
              Načíst další
            </Button>
            <Button color="primary" onClick={this.loadAll}>
              Načíst všechny
            </Button>
          </ButtonGroup>
        </div>
      </div>);

    return (
      <div>
        {contents}
      </div>
    );
  }
}

export default CommentList;
