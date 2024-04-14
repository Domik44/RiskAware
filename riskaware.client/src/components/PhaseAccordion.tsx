import { useState } from 'react';
import {
  Accordion,
  AccordionBody,
  AccordionHeader,
  AccordionItem,
  ListGroup,
  ListGroupItem,
  NavLink
} from 'reactstrap';
import IProjectDetail from './interfaces/IProjectDetail';

interface IPhaseAccordionProps {
  projectDetail: IProjectDetail;
  toggleTab: (tab: string) => void;
  chooseRisk: (id: number) => void;
}

function PhaseAccordion(props: IPhaseAccordionProps) {
  const [openItems, setOpenItems] = useState<string[]>([]);

  const toggle = (id: string) => {
    if (openItems.includes(id)) {
      setOpenItems(openItems.filter(item => item !== id));
    } else {
      setOpenItems([...openItems, id]);
    }
  };

  const renderAccordionItems = () => {
    if (!props.projectDetail) {
      return null;
    }

    return props.projectDetail.phases.map((phase) => (
      <AccordionItem key={phase.id}>
        <AccordionHeader targetId={phase.id.toString()} onClick={() => toggle(phase.id.toString())}>
          {phase.name}
        </AccordionHeader>
        <AccordionBody accordionId={phase.id.toString()} style={{ padding: 0 }}>
          <ListGroup>
            {phase.risks.map((risk) => (
              <NavLink onClick={() => props.chooseRisk(risk.id)} key={risk.id} className="clickable">
                <ListGroupItem>
                  {risk.title}
                </ListGroupItem>
              </NavLink>
            ))}
          </ListGroup>
        </AccordionBody>
      </AccordionItem>
    ));
  };

  return (
    <div>
      <Accordion open={openItems} toggle={toggle}>
        {renderAccordionItems()}
      </Accordion>
    </div>
  );
}

export default PhaseAccordion;
