import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';

interface AddRiskModalProps {
  // Define any props if needed
}
const AddRiskModal: React.FC<AddRiskModalProps> = () => {
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
      <Button color="success" onClick={toggle}> Přidat riziko </Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Přidání rizika</ModalHeader>
        <Form id="addRiskForm" onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label> Název:</Label>
                <Input id="title" name="title" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Popis:</Label>
                <Input id="description" name="description" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Pravďepodobnost:</Label>
                  <Input id="exampleSelect" name="select" type="select">
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
              </Col>
              <Col>
                <FormGroup>
                  <Label> Dopad:</Label>
                  <Input id="exampleSelect" name="select" type="select">
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
              </Col>
            </Row>
            <Row>
              <FormGroup>
                <Label>Hrozba:</Label>
                <Input type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Spouštěče:</Label>
                <Input type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Prevence:</Label>
                <Input type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Stav:</Label>
                <Input id="exampleSelect" name="select" type="select">
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

export default AddRiskModal;
