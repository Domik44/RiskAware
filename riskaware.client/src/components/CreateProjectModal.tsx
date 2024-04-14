import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';

interface CreateProjectModalProps {
  // Define any props if needed
  // here will be variable from the list page which will recieve new table/row
}

const CreateProjectModal: React.FC<CreateProjectModalProps> = () => {
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
      <Button color="success" onClick={toggle}>
        Přidat projekt
      </Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Tvorba projektu</ModalHeader>
        <Form id="createProjectForm" onSubmit={handleSubmit}>
          <ModalBody>
              <Row>
                <FormGroup>
                  <Label> Název projektu:</Label>
                  <Input id="title" name="title" type="text" />
                </FormGroup>
              </Row>
              <Row>
                <Col>
                  <FormGroup>
                    <Label> Začátek:</Label>
                    <Input id="start" name="start" type="date" />
                  </FormGroup>
                </Col>
                <Col>
                  <FormGroup>
                    <Label> Konec:</Label>
                    <Input id="end" name="end" type="date" />
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <FormGroup>
                  <Label> Projektový manažer:</Label>
                  {/*TODO -> change this later?*/}
                  <Input id="email" name="email" type="email" />
                </FormGroup>
               </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" type="submit">
              Vytvořit
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

export default CreateProjectModal;
