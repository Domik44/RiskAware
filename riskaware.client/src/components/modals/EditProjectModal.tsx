import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import IDetail from '../interfaces/IDetail';
import { formatDateForInput } from '../../common/DateFormatter';
import EditIcon from '@mui/icons-material/Edit';
import { Tooltip, IconButton } from '@mui/material';


interface EditProjectModalProps {
  projectId: number;
  reRender: () => void;
}

const EditProjectModal: React.FC<EditProjectModalProps> = ({ reRender, projectId }) => {
  const [modal, setModal] = useState(false);

  const toggle = () => setModal(!modal);
  const submit = async () => {
    try {
      const response = await fetch(`/api/RiskProject/${projectId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: (document.getElementById("EditProjectTitle") as HTMLInputElement).value,
          description: (document.getElementById("EditProjectDescription") as HTMLInputElement).value,
          start: (document.getElementById("EditProjectStart") as HTMLInputElement).value,
          end: (document.getElementById("EditProjectEnd") as HTMLInputElement).value,
        })
      });

      if (!response.ok) {
        if (response.status === 401) {
          throw new Error('* Nedostatečná oprávnění pro tuto akci!');
        }
        else {
          throw new Error('* Něco se pokazilo! Zkuste to prosím znovu.');
        }
      }
      else {
        reRender();
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
  }

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault(); // Prevent default form submission
    submit(); // Call the submit function
  }

  const openModal = async () => {
    try {
      const response = await fetch(`/api/RiskProject/${projectId}/Detail`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
          throw new Error('* Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        toggle();
        const data: IDetail = await response.json();
        (document.getElementById("EditProjectTitle") as HTMLInputElement).value = data.title;
        (document.getElementById("EditProjectDescription") as HTMLInputElement).value = data.description;
        (document.getElementById("EditProjectStart") as HTMLInputElement).value = formatDateForInput(data.start);
        (document.getElementById("EditProjectEnd") as HTMLInputElement).value = formatDateForInput(data.end);
      }
    }
    catch (error) {
      console.error('Error fetching project detail:', error);
    }
  }

  return (
    <div>
      <Tooltip title="Upravit detail">
        <IconButton color="primary" size="large" onClick={openModal}>
          <EditIcon fontSize="inherit" />
        </IconButton>
      </Tooltip>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Tvorba projektu</ModalHeader>
        <Form id="EditProjectForm" onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label> Název projektu:</Label>
                <Input required id="EditProjectTitle" name="EditProjectTitle" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Popis projektu:</Label>
                <Input id="EditProjectDescription" name="EditProjectDescription" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Začátek:</Label>
                  <Input required id="EditProjectStart" name="EditProjectStart" type="date" />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label> Konec:</Label>
                  <Input required id="EditProjectEnd" name="EditProjectEnd" type="date" />
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <p id="error" className="text-danger hidden"></p>
            </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" type="submit">
              Upravit
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

export default EditProjectModal;
