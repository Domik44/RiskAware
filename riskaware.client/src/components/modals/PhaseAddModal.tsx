import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import IProjectDetail from '../interfaces/IProjectDetail';
import IDtFetchData from '../interfaces/IDtFetchData';

interface AddPhaseModalProps {
  projectDetail: IProjectDetail;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const AddPhaseModal: React.FC<AddPhaseModalProps> = ({ projectDetail, reRender, fetchDataRef }) =>  {
  const [modal, setModal] = useState(false);
  const userRole = projectDetail.userRole;

  const toggle = () => setModal(!modal);
  const submit = async () => {
    const id = projectDetail.detail.id;
    const apiUrl = `/api/RiskProject/CreateProjectPhase`;
    try {
      const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: (document.getElementById("PhaseAddName") as HTMLInputElement).value,
          start: (document.getElementById("PhaseAddStart") as HTMLInputElement).value,
          end: (document.getElementById("PhaseAddEnd") as HTMLInputElement).value,
          userRoleType: userRole,
          riskProjectId: id
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page
        fetchDataRef.current?.();
      }
    }
    catch (error: any) {
      console.error(error);   // todo wrap in <ErrorBoundary FallBackComponent={showError}> tag
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
                <Input required id="PhaseAddName" name="PhaseAddName" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Začátek:</Label>
                  <Input required id="PhaseAddStart" name="PhaseAddStart" type="date" />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label> Konec:</Label>
                  <Input required id="PhaseAddEnd" name="PhaseAddEnd" type="date" />
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
