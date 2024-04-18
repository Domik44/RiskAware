import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import IProjectDetail from './interfaces/IProjectDetail';

interface AddPhaseModalProps {
  projectDetail: IProjectDetail;
  reRender: () => void;
}

const AddPhaseModal: React.FC<AddPhaseModalProps> = ({ projectDetail, reRender }) =>  {
  const [modal, setModal] = useState(false);
  const userRole = projectDetail.userRole;

  const toggle = () => setModal(!modal);
  const submit = async () => {
    const id = projectDetail.detail.id;
    const apiUrl = `/api/RiskProject/${id}/CreateProjectPhase`;
    try {
      const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: (document.getElementById("name") as HTMLInputElement).value,
          start: (document.getElementById("start") as HTMLInputElement).value,
          end: (document.getElementById("end") as HTMLInputElement).value,
          userRoleType: userRole
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page
      }
    }
    catch (error: any) {
      console.error(error);
    }
  }

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault(); // Prevent default form submission
    submit(); // Call the submit function
    toggle(); // Close the modal after submission
  }

  return (
    <div>
      <Button color="success" onClick={toggle}> Přidat fázi </Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Přidání fáze</ModalHeader>
        <Form id="addPhaseForm" onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label> Název:</Label>
                <Input required id="name" name="name" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Začátek:</Label>
                  <Input required id="start" name="start" type="date" />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label> Konec:</Label>
                  <Input required id="end" name="end" type="date" />
                </FormGroup>
              </Col>
            </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" type="submit">
              Přidat
            </Button>
            <Button color="secondary" onClick={toggle}>
              Zrušit
            </Button>
          </ModalFooter>
        </Form>
      </Modal>
    </div>
  );
}

export default AddPhaseModal;
