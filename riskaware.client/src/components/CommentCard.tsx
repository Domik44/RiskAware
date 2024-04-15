import { Card, CardBody, CardHeader, CardText } from "reactstrap";


interface CommentProps {
  username: string;
  date: string;
  text: string;
}

const CommentCard: React.FC<CommentProps> = ({ username, date, text }) => {
  return (
    <Card className="CommentCard">
      <CardHeader className="commentHeader">
          <div>{username}</div>
          <div>{date}</div>
      </CardHeader>
      <CardBody className="commentText">
        <CardText >
          {text}
        </CardText>
      </CardBody>
    </Card>
  );
};


export default CommentCard;
