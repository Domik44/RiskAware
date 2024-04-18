import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import { Impact, Prevention, Probability, Status, Category } from './enums/RiskAttributesEnum';
import IProjectDetail, { RoleType } from './interfaces/IProjectDetail';
import IRiskCategory from './interfaces/IRiskCategory';

interface AddRiskModalProps {
  projectDetail: IProjectDetail;
  reRender: () => void;
}

const AddRiskModal: React.FC<AddRiskModalProps> = ({ projectDetail, reRender }) => {
  const [modal, setModal] = useState(false);
  const [categories, setCategories] = useState<IRiskCategory[]>([]);
  const scale = projectDetail.detail.scale;
  const userRole = projectDetail.userRole;
  const assignedPhase = projectDetail.assignedPhase;

  const open = async () => {
    const id = projectDetail.detail.id;
    const apiUrl = `/api/RiskProject/${id}/RiskCategories`;
    try {
      const response = await fetch(apiUrl);

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        const data = await response.json();
        setCategories(data);
        console.log(categories);
      }
    }
    catch (error: any) {
      console.error(error);
    }
    toggle();
  };

  const toggle = () => {
    setModal(!modal);
  };

  const submit = async () => {
    const id = projectDetail.detail.id;
    const apiUrl = `/api/RiskProject/${id}/AddRisk`;
    const category = {
      id: parseInt((document.getElementById("category") as HTMLInputElement).value),
      name: (document.getElementById("newCategoryName") as HTMLInputElement).value
    }
    try {
      const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: (document.getElementById("title") as HTMLInputElement).value,
          description: (document.getElementById("description") as HTMLInputElement).value,
          probability: parseInt((document.getElementById("propability") as HTMLInputElement).value),
          impact: parseInt((document.getElementById("impact") as HTMLInputElement).value),
          threat: (document.getElementById("threat") as HTMLInputElement).value,
          indicators: (document.getElementById("indicators") as HTMLInputElement).value,
          prevention: (document.getElementById("prevention") as HTMLInputElement).value,
          status: (document.getElementById("status") as HTMLInputElement).value,
          preventionDone: (document.getElementById("preventionDone") as HTMLInputElement).value,
          riskEventOccured: (document.getElementById("riskOccured") as HTMLInputElement).value,
          end: (document.getElementById("end") as HTMLInputElement).value,
          projectPhaseId: parseInt((document.getElementById("phase") as HTMLInputElement).value),
          riskCategory: category,
          userRoleType: userRole
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page
      }
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

  const selectShow = (selectedCategory: number) => {
    const categoryGroup = document.getElementById("newCategoryGroup") as HTMLInputElement;
    const input = document.getElementById("newCategoryName") as HTMLInputElement;

    if (selectedCategory === Category.New) {
      categoryGroup.classList.remove("hidden");
      input.value = "";
    }
    else {
      categoryGroup.classList.add("hidden");
      input.value = "New";
    }
  };

  const handleSelectChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const selectedCategory = parseInt(event.target.value);
    selectShow(selectedCategory);
  };

  return (
    <div>
      <Button color="success" onClick={open}> Přidat riziko </Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Přidání rizika</ModalHeader>
        <Form id="addRiskForm" onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label> Fáze:</Label>
                {userRole === RoleType.TeamMember ?
                 (
                  <Input id="phase" name="phase" type="text" value={assignedPhase.name} readOnly />
                 ) : 
                 (
                   <Input id="phase" name="phase" type="select">
                    {projectDetail.phases.map((phase) => (
                      <option key={phase.id} value={phase.id}>
                        {phase.name}
                      </option>
                    ))}
                  </Input>
                 )
               }
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Název:</Label>
                <Input required id="title" name="title" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Popis:</Label>
                <Input id="description" name="description" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Kategorie:</Label>
                <Input id="category" name="category" type="select" onChange={handleSelectChange}>
                  <option id="newCategoryOption" value={Category.New}>
                    Nová kategorie
                  </option>
                  {categories.map((category) => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </Input>
              </FormGroup>
              <FormGroup id="newCategoryGroup">
                <Label> Název kategorie:</Label>
                <Input required id="newCategoryName" name="newCategoryName" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Pravděpodobnost:</Label>
                  <Input id="propability" name="propability" type="select">
                    {/*TODO -> fetch options from backend */}
                    <option value={Probability.Insignificant}>
                      Nepatrná
                    </option>
                    {scale === 5 && (
                      <option value={Probability.Minor}>
                        Malá
                      </option>
                    )}
                    <option value={Probability.Moderate}>
                      Střední
                    </option>
                    {scale === 5 && (
                      <option value={Probability.Major}>
                        Velká
                      </option>
                    )}
                    <option value={Probability.Severe}>
                      Závažná
                    </option>
                  </Input>
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label> Dopad:</Label>
                  <Input id="impact" name="impact" type="select">
                    <option value={Impact.Insignificant}>
                      Nepatrný
                    </option>
                    {scale === 5 && (
                      <option value={Impact.Minor}>
                        Malý
                      </option>
                    )}
                    <option value={Impact.Perceptible}>
                      Citelný
                    </option>
                    {scale === 5 && (
                      <option value={Impact.Critical}>
                        Kritický
                      </option>
                    )}
                    <option value={Impact.Catastrophic}>
                      Katastrofický
                    </option>
                  </Input>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <FormGroup>
                <Label>Hrozba:</Label>
                <Input id="threat" name="threat" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Spouštěče:</Label>
                <Input id="indicators" name="indicators" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Stav:</Label>
                  <Input id="status" name="status" type="select">
                    <option value={Status.Concept}>
                      Koncept
                    </option>
                    <option value={Status.Active}>
                      Aktivní
                    </option>
                    <option value={Status.Closed}>
                      Uzavřené
                    </option>
                    <option value={Status.Incident}>
                      Přihodilo se
                    </option>
                  </Input>
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label>Prevence:</Label>
                  <Input id="prevention" name="prevention" type="select">
                    <option value={Prevention.Neglect}>
                      Zanedbání
                    </option>
                    <option value={Prevention.Insurance}>
                      Pojištění
                    </option>
                    <option value={Prevention.Treatment}>
                      Ošetření
                    </option>
                  </Input>
                </FormGroup>
              </Col>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  {/*TODO -> handle na nevyplnene riziko*/}
                  <Label>Riziko nastalo:</Label> 
                  <Input id="riskOccured" name="riskOccured" type="date" />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label>Prevence provedena:</Label>
                  <Input required id="preventionDone" name="preventionDone" type="date" />
                </FormGroup>
              </Col>
            </Row>
              <FormGroup>
                <Label>Platnost rizika:</Label>
              <Input required id="end" name="end" type="date" />
              </FormGroup>
            <Row>
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
