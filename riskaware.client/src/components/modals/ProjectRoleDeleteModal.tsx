import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import IDtFetchData from '../interfaces/IDtFetchData';

interface ProjectDeleteModalProps {
  projectRoleId: number;
  isOpen: boolean;
  toggle: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const ProjectDeleteModal: React.FC<ProjectDeleteModalProps> = ({ projectRoleId, isOpen, toggle, fetchDataRef }) => {
  const deleteProjectRole = async () => {
    try {
      const response = await fetch(`/api/ProjectRole/${projectRoleId}`, {
        method: 'DELETE'
      });

      if (!response.ok) {
        const alert = document.getElementById('notDeleted');
        const errMsg = await response.text();
        if (alert) {
          alert.classList.remove('hidden');
          alert.innerHTML = errMsg;
        }
        throw new Error(errMsg);
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
      <ModalHeader toggle={toggle}>Odstranění uživatele z projektu</ModalHeader>
      <ModalBody>
        <Alert className="alert alert-warning">
          Opravdu chcete odstranit uživatele z projektu?
          Uživatel bude odstraněn pouze pokuď nemá žádné vytvořené rizika.
        </Alert>
        <Alert id="notDeleted" className="hidden alert alert-danger">
          
        </Alert>
      </ModalBody>
      <ModalFooter className="d-flex align-items-center justify-content-center">
        <Button color="danger" onClick={deleteProjectRole}>
          Vymazat
        </Button>
        <Button color="secondary" onClick={toggle}>
          Zrušit
        </Button>
      </ModalFooter>
    </Modal>
  );
}

export default ProjectDeleteModal;
