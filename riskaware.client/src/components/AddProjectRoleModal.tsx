import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input} from 'reactstrap';
import IProjectDetail, { RoleType } from './interfaces/IProjectDetail';

interface AddProjectRoleModalProps {
  projectDetail: IProjectDetail;
  // Define any props if needed
}

const AddProjectRoleModal: React.FC<AddProjectRoleModalProps> = ({ projectDetail }) => {
  const [modal, setModal] = useState(false);
  const userRole = projectDetail.userRole;

  const toggle = () => setModal(!modal);
  const submit = async () => {
    const id = projectDetail.detail.id;
    const apiUrl = `/api/RiskProject/${id}/AddUserToRiskProject`;
    try { 
      const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: (document.getElementById("email") as HTMLInputElement).value,
          roleType: parseInt((document.getElementById("roleType") as HTMLInputElement).value),
          name: (document.getElementById("name") as HTMLInputElement).value,
          userRoleType: userRole
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }

      // TODO -> fetch members again
      // will add after the tables are implemented
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

  const selectFill = (selectedRole: number) => {
    const nameInput = document.getElementById("name") as HTMLInputElement;
    const phaseSelect = document.getElementById("phaseSelect") as HTMLInputElement;

    phaseSelect.classList.add("hidden");
    if (selectedRole == RoleType.RiskManager) {
      nameInput.value = "Rizikový manažer";
    }
    else if (selectedRole == RoleType.TeamMember) {
      nameInput.value = "Člen týmu";
      // TODO here show the phase select
      phaseSelect.classList.remove("hidden");
    }
    else {
      nameInput.value = "Externí člen";
    }
  };

  const handleSelectChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const selectedRoleType = parseInt(event.target.value);
    selectFill(selectedRoleType);
  };

  return (
    <div>
      <Button color="success" onClick={toggle}> Přidat uživatele </Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Přidání uživatele k projektu</ModalHeader>
        <Form id="addPhaseForm" onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label>Výběr uživatele:</Label>
                <Input required id="email" name="email" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Typ role:</Label>
                <Input required id="roleType" name="roleType" type="select" onChange={handleSelectChange}>
                  <option value={RoleType.RiskManager}>
                    Rizikový manažer
                  </option>
                  <option value={RoleType.TeamMember}>
                    Člen týmu
                  </option>
                  <option value={RoleType.ExternalMember}>
                    Externí člen
                  </option>
                </Input>
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Pojmenování role:</Label>
                <Input required id="name" name="name" type="text" defaultValue="Rizikový manažer" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Přiřazená fáze:</Label>
                <Input id="phaseSelect" name="select" type="select" className="hidden">
                  {/*TODO -> now i can assigned already assigned phases -> FIX -> fetch only free phases*/}
                  {projectDetail.phases.map((phase) => (
                    <option key={phase.id} value={phase.id}>
                      {phase.name}
                    </option>
                  ))}
                </Input>
              </FormGroup>
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

export default AddProjectRoleModal;
