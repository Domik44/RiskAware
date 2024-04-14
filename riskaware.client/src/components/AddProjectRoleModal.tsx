import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input} from 'reactstrap';

interface AddProjectRoleModalProps {
  // Define any props if needed
}

const AddProjectRoleModal: React.FC<AddProjectRoleModalProps> = () => {
  const [modal, setModal] = useState(false);

  // TODO -> mby clean data on closing?
  //const open = () => setModal(true);
  //const close = () => {
  //  // Close the modal
  //}
  const toggle = () => setModal(!modal);
  const submit = () => {
    console.log("Submit form data here");
    // here will be fetch to the backend
    // then clean the form
    // then reload the page ?
  }

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault(); // Prevent default form submission
    submit(); // Call the submit function
    toggle(); // Close the modal after submission
  }

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
                <Input id="email" name="email" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Typ role:</Label>
                <Input id="roleTypeSelect" name="roleType" type="select">
                  {/*TODO -> fetch options from backend */}
                  <option>
                    1
                  </option>
                  <option>
                    2
                  </option>
                  <option>
                    3
                  </option>
                  <option>
                    4
                  </option>
                  <option>
                    5
                  </option>
                </Input>
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Pojmenování role:</Label>
                {/*TODO -> pre fill with role type name*/}
                <Input id="name" name="name" type="text" />
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
