import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import IDtFetchData from '../interfaces/IDtFetchData';

interface PhaseDeleteModalProps {
  phaseId: number;
  isOpen: boolean;
  toggle: () => void;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const PhaseDeleteModal: React.FC<PhaseDeleteModalProps> = ({ phaseId, isOpen, toggle, reRender, fetchDataRef }) => {
  const deletePhase = async () => {
    try {
      const response = await fetch(`/api/ProjectPhase/${phaseId}`, {
        method: 'DELETE'
      });

      if (!response.ok) {
        if (response.status === 400) {
          document.getElementById('notDeleted')?.classList.remove('hidden');
          throw new Error('Fáze obsahuje rizika!');
        }
        else {
          throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
        }
      }
      else {
        reRender(); // Rerender the page
        fetchDataRef.current?.();
        toggle();
        document.getElementById('notDeleted')?.classList.add('hidden');
      }

    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Vymazání fáze</ModalHeader>
      <ModalBody>
        <Alert className="alert alert-warning">
          Opravdu chcete vymazat fázi?
          Vymazat fázi lze pouze pokud neobsahuje žádná rizika.
        </Alert>
        <Alert id="notDeleted" className="hidden alert alert-danger">
          Fáze obsahuje rizika! Smazání neprošlo.
        </Alert>
      </ModalBody>
      <ModalFooter className="d-flex align-items-center justify-content-center">
        <Button color="danger" onClick={deletePhase}>
          Vymazat
        </Button>
        <Button color="secondary" onClick={toggle}>
          Zrušit
        </Button>
      </ModalFooter>
    </Modal>
  );
}

export default PhaseDeleteModal;
