import React from 'react';
import { Accordion, Icon } from 'semantic-ui-react';

import './AccordionUI.css';

export const MyAccordion = (props) => {
    let accordionSections = props.accordionSections;
    accordionSections = accordionSections?.map((accordionSections, index) => {
        return (
            <>
                <Accordion.Title active={props.activeIndex === index} index={index} onClick={props.onClick}>
                    <Icon name="dropdown" />
                    {accordionSections.title}
                </Accordion.Title>
                <Accordion.Content active={props.activeIndex === index}>{accordionSections.content}</Accordion.Content>
            </>
        );
    });

    return (
        <Accordion className="MyAccordion" styled>
            {accordionSections}
        </Accordion>
    );
};
