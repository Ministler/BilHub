import React, { Component } from 'react';
import { Accordion, Icon } from 'semantic-ui-react';

import './Accordion.css';

export class MyAccordion extends Component {
    state = { activeIndex: 0 };

    handleClick = (e, titleProps) => {
        const { index } = titleProps;
        const { activeIndex } = this.state;
        const newIndex = activeIndex === index ? -1 : index;

        this.setState({ activeIndex: newIndex });
    };

    render() {
        const { activeIndex } = this.state;

        let accordionElements = this.props.accordionElements;
        accordionElements = accordionElements.map((element, index) => {
            return (
                <>
                    <Accordion.Title active={activeIndex === index} index={index} onClick={this.handleClick}>
                        <Icon name="dropwdown" />
                        {element.title}
                    </Accordion.Title>
                    <Accordion.Content active={activeIndex === index}>{element.content}</Accordion.Content>
                </>
            );
        });

        return <Accordion>{accordionElements}</Accordion>;
    }
}
