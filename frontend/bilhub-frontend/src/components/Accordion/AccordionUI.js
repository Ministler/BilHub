import React, { Component } from 'react';
import { Accordion, Icon } from 'semantic-ui-react';

import './AccordionUI.css';

export class MyAccordion extends Component {
    constructor(props) {
        super(props);
        this.state = {
            activeIndex: 0,
        };
    }

    handleClick = (e, titleProps) => {
        const { index } = titleProps;
        const { activeIndex } = this.state;
        const newIndex = activeIndex === index ? -1 : index;

        this.setState({
            activeIndex: newIndex,
        });
    };

    render() {
        let accordionSections = this.props.accordionSections;
        accordionSections = accordionSections?.map((accordionSections, index) => {
            return (
                <>
                    <Accordion.Title active={this.state.activeIndex === index} index={index} onClick={this.handleClick}>
                        <Icon name="dropdown" />
                        {accordionSections.title}
                    </Accordion.Title>
                    <Accordion.Content active={this.state.activeIndex === index}>
                        {accordionSections.content}
                    </Accordion.Content>
                </>
            );
        });

        return (
            <Accordion className="MyAccordion">
                {accordionSections}
            </Accordion>
        );
    }
}
