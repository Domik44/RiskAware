import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import IDtFetchData from '../interfaces/IDtFetchData';

interface RiskDeleteModalProps {
  riskId: number;
  isOpen: boolean;
  toggle: () => void;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const RiskDeleteModal: React.FC<RiskDeleteModalProps> = ({ riskId, isOpen, toggle, reRender, fetchDataRef }) => {
  const deleteRisk = async () => {
    try {
      const response = await fetch(`/api/Risk/${riskId}`, {
        method: 'DELETE'
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page
        fetchDataRef.current?.(); // Fetch data again
        toggle();
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Vymazání rizika</ModalHeader>
      <ModalBody>
        <Alert className="alert alert-warning">
          Opravdu chcete vymazat riziko?
          Riziko bude nastaveno jako nevalidní.
        </Alert>
      </ModalBody>
      <ModalFooter className="d-flex align-items-center justify-content-center">
        <Button color="danger" onClick={deleteRisk}>
          Vymazat
        </Button>
        <Button color="secondary" onClick={toggle}>
          Zrušit
        </Button>
      </ModalFooter>
    </Modal>
  );
}

export default RiskDeleteModal;
