import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert, Form, Row, Col, FormGroup, Label, Input } from 'reactstrap';
import IFetchData from '../common/IFetchData';
import IPhases from './interfaces/IPhases';
import { formatDateForInput } from "../helpers/DateFormatter";

interface PhaseEditModalProps {
  phaseId: number;
  isOpen: boolean;
  toggle: () => void;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IFetchData | null>;
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
        //if (response.status === 400) {
        //  document.getElementById('notEditd')?.classList.remove('hidden');
        //  throw new Error('Fáze obsahuje rizika!');
        //}
        //else {
        //}
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page
        fetchDataRef.current?.();
        toggle();
        //document.getElementById('notEditd')?.classList.add('hidden');
      }

    } catch (error) {
      console.error(error);
    }
  };

  // TODO -> fill input data from backend
  return (
    <div>
      <Modal isOpen={isOpen} toggle={toggle}>
        <ModalHeader toggle={toggle}>Upravit fázi</ModalHeader>
        <Form id="editPhaseForm" onSubmit={editPhase}>
          <ModalBody>
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

