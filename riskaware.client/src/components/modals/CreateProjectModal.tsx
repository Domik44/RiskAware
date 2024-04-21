import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import IDtFetchData from '../interfaces/IDtFetchData';

interface CreateProjectModalProps {
  // Define any props if needed
  // here will be variable from the list page which will recieve new table/row
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const CreateProjectModal: React.FC<CreateProjectModalProps> = ({ fetchDataRef }) => {
  const [modal, setModal] = useState(false);

  const toggle = () => setModal(!modal);
  const submit = async () => {
    // TODO -> data validation -> check if email exists in the database
    try {
      const response = await fetch('/api/RiskProject', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: (document.getElementById("CreateProjectTitle") as HTMLInputElement).value,
          start: (document.getElementById("CreateProjectStart") as HTMLInputElement).value,
          end: (document.getElementById("CreateProjectEnd") as HTMLInputElement).value,
          email: (document.getElementById("CreateProjectEmail") as HTMLInputElement).value
        })
      });

      if (!response.ok) {
        if (response.status === 404) {
          throw new Error('* Uživatel zvolený pro roli projektového manažera nenalezen!'); // TODO -> pre check before submit??
        }
        else if (response.status === 401) {
          throw new Error('* Nedostatečná oprávnění pro tuto akci!');
        }
        else {
          throw new Error('* Něco se pokazilo! Zkuste to prosím znovu.');
        }
      }
      else {
        fetchDataRef.current?.();
        toggle(); // Close the modal after submission
      }
    }
    catch (error: any) {
      var errorElement = document.getElementById("error");
      if (errorElement) {
        errorElement.innerHTML = error.toString().substring(7);
        errorElement.classList.remove("hidden");
      }
    }

    fetchDataRef.current?.();
  }

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault(); // Prevent default form submission
    submit(); // Call the submit function
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
                  <Input required id="CreateProjectTitle" name="CreateProjectTitle" type="text" />
                </FormGroup>
              </Row>
              <Row>
                <Col>
                  <FormGroup>
                    <Label> Začátek:</Label>
                  <Input required id="CreateProjectStart" name="CreateProjectStart" type="date" />
                  </FormGroup>
                </Col>
                <Col>
                  <FormGroup>
                    <Label> Konec:</Label>
                  <Input required id="CreateProjectEnd" name="CreateProjectEnd" type="date" />
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <FormGroup>
                  <Label> Projektový manažer:</Label>
                  {/*TODO -> change this later?*/}
                <Input required id="CreateProjectEmail" name="CreateProjectEmail" type="email" />
                </FormGroup>
              </Row>
              <Row>
                <p id="error" className="text-danger hidden"></p>
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
