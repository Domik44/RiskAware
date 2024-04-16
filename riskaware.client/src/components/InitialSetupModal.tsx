import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import IProjectDetail, { RoleType } from './interfaces/IProjectDetail';
import { useNavigate } from "react-router-dom";
import { formatDateForInput } from "../helpers/DateFormatter";

interface InitialSetupModalProps {
  projectDetail: IProjectDetail;
}

const InitialSetupModal: React.FC<InitialSetupModalProps> = ({ projectDetail }) => {
  const [modal, setModal] = useState(true);
  const userRole = projectDetail.userRole;
  const navigate = useNavigate();

  /**
   * Open/Close modal.
   */
  const toggle = () => setModal(!modal);

  /***
   * Close the modal and navigate back handler.
   */
  const exit = () => {
    toggle();
    navigate(-1);
  }

  /**
   * Submit the form data.
   */
  const submit = async () => {
    try {
      const id = projectDetail.detail.id;
      const apiUrl = `/api/RiskProject/${id}/InitialRiskProjectSetup`;
      console.log(apiUrl);
      const response = await fetch(apiUrl, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: (document.getElementById("title") as HTMLInputElement).value,
          description: (document.getElementById("description") as HTMLInputElement).value,
          start: (document.getElementById("start") as HTMLInputElement).value,
          end: (document.getElementById("end") as HTMLInputElement).value,
          scale: (document.getElementById("scale") as HTMLInputElement).value
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        navigate(0);
      }
    }
    catch (error: any) {
      console.error(error);
    }
  }

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault(); // Prevent default form submission
    submit(); // Call the submit function
  }

  const form = (
    <div>
      <Row>
        {/*TODO -> predvyplnit s daty z projektu*/}
        <FormGroup>
          <Label> Název projektu:</Label>
          <Input required id="title" name="title" type="text" defaultValue={projectDetail.detail.title} />
        </FormGroup>
      </Row>
      <Row>
        <FormGroup>
          <Label> Popis projektu:</Label>
          <Input id="description" name="description" type="textarea" />
        </FormGroup>
      </Row>
      <Row>
        {/*TODO -> predvyplnit s daty z projektu*/}
        <Col>
          <FormGroup>
            <Label> Začátek:</Label>
            <Input id="start" name="start" type="date" defaultValue={formatDateForInput(projectDetail.detail.start)} />
          </FormGroup>
        </Col>
        <Col>
          <FormGroup>
            <Label> Konec:</Label>
            <Input id="end" name="end" type="date" defaultValue={formatDateForInput(projectDetail.detail.end)} />
          </FormGroup>
        </Col>
      </Row>
      <Row>
        <FormGroup>
          <Label> Škála matice:</Label>
          <Input required id="scale" name="scale" type="select">
            <option>
              3
            </option>
            <option>
              5
            </option>
          </Input>
        </FormGroup>
      </Row>
    </div>

  );
  const message = (
    <Row>
      <p className="text">Projekt ještě nebyl nastaven.</p>
      <p className="text">Kontaktujte projektového manažera.</p>
    </Row>
  );

  const contentToRender = userRole === RoleType.CommonUser ? message : form;

  return (
    <div>
      <Modal isOpen={modal} toggle={toggle} backdrop="static" keyboard={false}>
        <ModalHeader toggle={exit}>Nastavení projektu</ModalHeader>
        <Form id="createProjectForm" onSubmit={handleSubmit}>
          <ModalBody>
            {contentToRender}
          </ModalBody>
          <ModalFooter>
            {userRole !== RoleType.CommonUser && (
            <Button color="primary" type="submit">
              Nastavit
            </Button>
            )}
            <Button color="secondary" onClick={exit}>
              Odejít
            </Button>
          </ModalFooter>
        </Form>
      </Modal>
    </div>
  );
}

export default InitialSetupModal;
