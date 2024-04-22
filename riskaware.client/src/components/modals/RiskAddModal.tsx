import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import { Impact, Prevention, Probability, Status, Category } from '../enums/RiskAttributesEnum';
import IProjectDetail, { RoleType } from '../interfaces/IProjectDetail';
import IRiskCategory from '../interfaces/IRiskCategory';
import IDtFetchData from '../interfaces/IDtFetchData';
import { DatePicker } from '@mui/x-date-pickers';
import { parseCzechDate } from '../../common/DateFormatter';

interface AddRiskModalProps {
  projectDetail: IProjectDetail;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
}

const AddRiskModal: React.FC<AddRiskModalProps> = ({ projectDetail, reRender, fetchDataRef }) => {
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
      id: parseInt((document.getElementById("RiskAddCategory") as HTMLInputElement).value),
      name: (document.getElementById("newCategoryName") as HTMLInputElement).value
    }
    try {
      const preventionDone = (document.querySelector('input[name="RiskAddPreventionDone"]') as HTMLInputElement).value;
      console.log(preventionDone);
      const riskOccured = (document.querySelector('input[name="RiskAddRiskOccured"]') as HTMLInputElement).value;
      const end = (document.querySelector('input[name="RiskAddEnd"]') as HTMLInputElement).value;
      const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: (document.getElementById("RiskAddTitle") as HTMLInputElement).value,
          description: (document.getElementById("RiskAddDescription") as HTMLInputElement).value,
          probability: parseInt((document.getElementById("RiskAddPropability") as HTMLInputElement).value),
          impact: parseInt((document.getElementById("RiskAddImpact") as HTMLInputElement).value),
          threat: (document.getElementById("RiskAddThreat") as HTMLInputElement).value,
          indicators: (document.getElementById("RiskAddIndicators") as HTMLInputElement).value,
          prevention: (document.getElementById("RiskAddPrevention") as HTMLInputElement).value,
          status: (document.getElementById("RiskAddStatus") as HTMLInputElement).value,
          preventionDone: preventionDone === "" ? "0001-01-01" : parseCzechDate(preventionDone),
          riskEventOccured: riskOccured === "" ? "0001-01-01" : parseCzechDate(riskOccured),
          end: parseCzechDate(end),
          projectPhaseId: parseInt((document.getElementById("RiskAddPhase") as HTMLInputElement).value),
          riskCategory: category,
          userRoleType: userRole
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page
        fetchDataRef.current?.();
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
                    <Input id="RiskAddPhase" name="RiskAddPhase" type="text" value={assignedPhase.name} readOnly />
                  ) :
                  (
                    <Input id="RiskAddPhase" name="RiskAddPhase" type="select">
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
                <Input required id="RiskAddTitle" name="RiskAddTitle" type="text" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Popis:</Label>
                <Input id="RiskAddDescription" name="RiskAddDescription" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Kategorie:</Label>
                <Input id="RiskAddCategory" name="RiskAddCategory" type="select" onChange={handleSelectChange}>
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
                  <Input id="RiskAddPropability" name="RiskAddPropability" type="select">
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
                  <Input id="RiskAddImpact" name="RiskAddImpact" type="select">
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
                <Input id="RiskAddThreat" name="RiskAddThreat" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Spouštěče:</Label>
                <Input id="RiskAddIndicators" name="RiskAddIndicators" type="textarea" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Stav:</Label>
                  <Input id="RiskAddStatus" name="RiskAddStatus" type="select">
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
                  <Input id="RiskAddPrevention" name="RiskAddPrevention" type="select">
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
                  <DatePicker className="form-control" name="RiskAddRiskOccured" />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label>Prevence provedena:</Label>
                  <DatePicker className="form-control" name="RiskAddPreventionDone" />
                </FormGroup>
              </Col>
            </Row>
            <FormGroup>
              <Label>Platnost rizika:</Label>
              <DatePicker
                className="form-control"
                name="RiskAddEnd"
                minDate={new Date("2024-04-10")}
                maxDate={new Date("2024-05-05")}
                slotProps={{
                  textField: {
                    required: true,
                  },
                }} />
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
