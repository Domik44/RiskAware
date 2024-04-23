import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import IDtFetchData from '../interfaces/IDtFetchData';

interface DeleteProjectModalProps {
  riskProjectId: number;
  isOpen: boolean;
  toggle: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const DeleteProjectModal: React.FC<DeleteProjectModalProps> = ({ riskProjectId, isOpen, toggle, fetchDataRef }) => {
  const deleteRiskProject = async () => {
    try {
      const response = await fetch(`/api/RiskProject/${riskProjectId}`, {
        method: 'DELETE'
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        fetchDataRef.current?.(); // Fetch data again
        toggle();
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Vymazání projektu</ModalHeader>
      <ModalBody>
        <Alert className="alert alert-warning">
          Opravdu chcete vymazat projekt?
          Projekt bude znevalidněn a nebude možné na něj nahlížet.
          Projekt je možné obnovit.
        </Alert>
      </ModalBody>
      <ModalFooter className="d-flex align-items-center justify-content-center">
        <Button color="danger" onClick={deleteRiskProject}>
          Vymazat
        </Button>
        <Button color="secondary" onClick={toggle}>
          Zrušit
        </Button>
      </ModalFooter>
    </Modal>
  );
}

export default DeleteProjectModal;
