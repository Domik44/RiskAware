import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

interface RiskDeleteModalProps {
  riskId: number;
  isOpen: boolean;
  toggle: () => void;
}

const RiskDeleteModal: React.FC<RiskDeleteModalProps> = ({ riskId, isOpen, toggle }) => {
  const deleteRisk = async () => {
    try {
      const response = await fetch(`/api/Risk/${riskId}/Delete`);
      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
    } catch (error) {
      console.error(error);
    }
    toggle();
  };
  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Vymazání rizika</ModalHeader>
      <ModalBody>
        Opravdu chcete vymazat riziko?
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
