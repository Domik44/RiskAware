import { useState } from 'react';
import {
  Accordion,
  AccordionBody,
  AccordionHeader,
  AccordionItem,
} from 'reactstrap';
import IProjectDetail from './interfaces/IProjectDetail';

interface IPhaseAccordionProps {
  projectDetail: IProjectDetail;
}

function PhaseAccordion(props: IPhaseAccordionProps) {
  const [openItems, setOpenItems] = useState<string[]>([]);

  const toggle = (id: string) => {
    if (openItems.includes(id)) {
      // If the item is already open, close it
      setOpenItems(openItems.filter(item => item !== id));
    } else {
      // If the item is closed, add it to the open items
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
        <AccordionBody accordionId={phase.id.toString()}>
          <ul>
            {phase.risks.map((risk) => (
              <li key={risk.id}>
                <p>{risk.title}</p>
              </li>
            ))}
          </ul>
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
