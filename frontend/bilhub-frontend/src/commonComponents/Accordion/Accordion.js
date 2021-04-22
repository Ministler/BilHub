import React, { Component } from 'react';
import { Accordion, Icon } from 'semantic-ui-react';

import './Accordion.css';

export const MyAccordion = (props) => {
    const activeIndex = props.activeIndex;

    let accordionElements = props.accordionElements;
    accordionElements = accordionElements.map((element, index) => {
        return (
            <>
                <Accordion.Title active={activeIndex === index} index={index} onClick={props.handleClick}>
                    <Icon name="dropwdown" />
                    {element.title}
                </Accordion.Title>
                <Accordion.Content active={activeIndex === index}>{element.content}</Accordion.Content>
            </>
        );
    });

    return <Accordion>{accordionElements}</Accordion>;
};
