import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert, Form, Row, Col, FormGroup, Label, Input } from 'reactstrap';
import IDtFetchData from '../interfaces/IDtFetchData';
import IPhases from '../interfaces/IPhases';
import { formatDateForInput } from "../../common/DateFormatter";

interface PhaseEditModalProps {
  phaseId: number;
  isOpen: boolean;
  toggle: () => void;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
  data: IPhases | undefined;
  projectId: number;
}

const PhaseEditModal: React.FC<PhaseEditModalProps> = ({ phaseId, isOpen, toggle, reRender, fetchDataRef, data, projectId }) => {

  const editPhase = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      const response = await fetch(`/api/ProjectPhase/${phaseId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: (document.getElementById("editPhaseName") as HTMLInputElement).value,
          start: (document.getElementById("editPhaseStart") as HTMLInputElement).value,
          end: (document.getElementById("editPhaseEnd") as HTMLInputElement).value,
          riskProjectId: projectId
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page -> for phase accordion
        fetchDataRef.current?.(); // Fetch table data
        toggle(); // Close the modal after submission
      }

    } catch (error) {
      document.getElementById('editPhaseModalError')?.classList.remove('hidden');
    }
  };

  // TODO -> fill input data from backend
  return (
    <div>
      <Modal isOpen={isOpen} toggle={toggle}>
        <ModalHeader toggle={toggle}>Upravit fázi</ModalHeader>
        <Form id="editPhaseForm" onSubmit={editPhase}>
          <ModalBody>
            <Alert className="hidden alert alert-danger" id="editPhaseModalError">
              <p id="editPhaseModalErrorMsg"> Editace fáze se nezdařila! </p>
            </Alert>
            <Row>
              <FormGroup>
                <Label> Název:</Label>
                <Input required id="editPhaseName" name="editPhaseName" type="text" defaultValue={data?.name} />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Začátek:</Label>
                  <Input required id="editPhaseStart" name="editPhaseStart" type="date" defaultValue={data ? formatDateForInput(data.start) : ''} />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label> Konec:</Label>
                  <Input required id="editPhaseEnd" name="editPhaseEnd" type="date" defaultValue={data ? formatDateForInput(data.end) : ''} />
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

export default PhaseEditModal;

