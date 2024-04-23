import React from 'react';
import IDtFetchData from "../interfaces/IDtFetchData";
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';
import { Impact, Prevention, Probability, Status, Category } from '../enums/RiskAttributesEnum';
import IProjectDetail, { RoleType } from '../interfaces/IProjectDetail';
import IRiskCategory from '../interfaces/IRiskCategory';
import IRiskEdit from '../interfaces/IRiskEdit';
import { formatDateForInput } from '../../common/DateFormatter';
//import IRiskDetail from '../interfaces/IRiskDetail';


interface RiskEditModalProps {
  riskId: number;
  isOpen: boolean;
  toggle: () => void;
  reRender: () => void;
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>;
  data: IRiskEdit | undefined;
  //data: IRiskDetail | undefined;
  projectDetail: IProjectDetail;
  categories: IRiskCategory[];
}

const RiskEditModal: React.FC<RiskEditModalProps> = ({ riskId, isOpen, toggle, reRender, fetchDataRef, data, projectDetail, categories }) => {
  const scale = projectDetail.detail.scale;
  const userRole = projectDetail.userRole;
  const assignedPhase = projectDetail.assignedPhase;
  //const [categories, setCategories] = useState<IRiskCategory[]>([]);

  const editRisk = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      const preventionDone = (document.querySelector('input[name="RiskEditPreventionDone"]') as HTMLInputElement).value;
      const riskOccured = (document.querySelector('input[name="RiskEditRiskOccured"]') as HTMLInputElement).value;
      const end = (document.querySelector('input[name="RiskEditEnd"]') as HTMLInputElement).value;
      const response = await fetch(`/api/Risk/${riskId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: (document.getElementById("RiskEditTitle") as HTMLInputElement).value,
          description: (document.getElementById("RiskEditDescription") as HTMLInputElement).value,
          probability: parseInt((document.getElementById("RiskEditPropability") as HTMLInputElement).value),
          impact: parseInt((document.getElementById("RiskEditImpact") as HTMLInputElement).value),
          threat: (document.getElementById("RiskEditThreat") as HTMLInputElement).value,
          indicators: (document.getElementById("RiskEditIndicators") as HTMLInputElement).value,
          prevention: (document.getElementById("RiskEditPrevention") as HTMLInputElement).value,
          status: (document.getElementById("RiskEditStatus") as HTMLInputElement).value,
          preventionDone: preventionDone === "" ? "0001-01-01" : preventionDone,
          riskEventOccured: riskOccured === "" ? "0001-01-01" : riskOccured,
          end: end === "" ? "0001-01-01" : end,
          riskCategory: {
            id: parseInt((document.getElementById("RiskEditCategory") as HTMLInputElement).value),
            name: (document.getElementById("newCategoryName") as HTMLInputElement).value
          },
          projectPhaseId: parseInt((document.getElementById("RiskEditPhase") as HTMLInputElement).value)
        })
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        reRender(); // Rerender the page -> for phase accordion
        fetchDataRef.current?.(); // Fetch table data
        toggle(); // Close the modal after submission
      }

    } catch (error) {
      //document.getElementById('editRiskModalError')?.classList.remove('hidden');
    }
  };

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

  const handleDateInput = (date: Date | string | undefined) => {
    if (date) {
      // Check if date is a string
      if (typeof date === "string") {
        // Parse the string to a Date object
        date = new Date(date);
      }

      // Check if date is a valid Date object
      if (date instanceof Date && !isNaN(date.getTime())) {
        const zeroDate = new Date("0001-01-01T00:00:00");

        if (date.getTime() === zeroDate.getTime()) {
          return undefined;
        }
      }
      return formatDateForInput(date);
    }

    return undefined;
  };

  return (
    <div>
      <Modal isOpen={isOpen} toggle={toggle}>
        <ModalHeader toggle={toggle}>Úprava rizika</ModalHeader>
        <Form id="editRiskForm" onSubmit={editRisk}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label> Fáze:</Label>
                {userRole === RoleType.TeamMember ?
                  (
                    <div>
                      <Input id="RiskEditPhaseName" name="RiskEditPhaseName" type="text" value={assignedPhase?.name} readOnly />
                      <Input id="RiskEditPhase" name="RiskEditPhase" type="number" value={assignedPhase?.id} readOnly className="hidden" />
                    </div>                  ) :
                  (
                    <Input id="RiskEditPhase" name="RiskEditPhase" type="select" defaultValue={data?.projectPhaseId}>
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
                <Input required id="RiskEditTitle" name="RiskEditTitle" type="text" defaultValue={data?.title} />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Popis:</Label>
                <Input id="RiskEditDescription" name="RiskEditDescription" type="textarea" defaultValue={data?.description} />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label> Kategorie:</Label>
                <Input id="RiskEditCategory" name="RiskEditCategory" type="select" onChange={handleSelectChange} defaultValue={data?.riskCategory.id}>
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
              <FormGroup id="newCategoryGroup" className="hidden">
                <Label> Název kategorie:</Label>
                <Input required id="newCategoryName" name="newCategoryName" type="text" defaultValue="New" />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Pravděpodobnost:</Label>
                  <Input id="RiskEditPropability" name="RiskEditPropability" type="select" defaultValue={data?.probability}>
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
                  <Input id="RiskEditImpact" name="RiskEditImpact" type="select" defaultValue={data?.impact}>
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
                <Input id="RiskEditThreat" name="RiskEditThreat" type="textarea" defaultValue={data?.threat} />
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>Spouštěče:</Label>
                <Input id="RiskEditIndicators" name="RiskEditIndicators" type="textarea" defaultValue={data?.indicators} />
              </FormGroup>
            </Row>
            <Row>
              <Col>
                <FormGroup>
                  <Label> Stav:</Label>
                  <Input id="RiskEditStatus" name="RiskEditStatus" type="select" defaultValue={data?.status}>
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
                  <Input id="RiskEditPrevention" name="RiskEditPrevention" type="select" defaultValue={data?.prevention}>
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
                  <Input id="RiskEditRiskOccured" name="RiskEditRiskOccured" type="date" defaultValue={handleDateInput(data?.riskEventOccured)} />
                </FormGroup>
              </Col>
              <Col>
                <FormGroup>
                  <Label>Prevence provedena:</Label>
                  <Input id="RiskEditPreventionDone" name="RiskEditPreventionDone" type="date" defaultValue={handleDateInput(data?.preventionDone)} />
                </FormGroup>
              </Col>
            </Row>
            <FormGroup>
              <Label>Platnost rizika:</Label>
              <Input id="RiskEditEnd" name="RiskEditEnd" type="date" defaultValue={handleDateInput(data?.end)} />
            </FormGroup>
            <Row>
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

export default RiskEditModal;
