import React from 'react';
import { Icon, Card } from 'semantic-ui-react';

import './CardTypes.css';

/* Expects list of divs as a prop */
export const AssignmentCardElement = (props) => {
    let titleIcon = null;
    if (props.status === 'graded') {
        titleIcon = <Icon name="check circle outline" />;
    } else if (props.status === 'submitted') {
        titleIcon = <Icon name="clock outline" />;
    } else if (props.status === 'notsubmitted') {
        titleIcon = <Icon name="remove circle" />;
    }
    return (
        <Card className="AssignmentCard">
            <Card.Content>
                <Card.Header>
                    <span className="clickableChangeColor" onClick={props.titleClicked}>
                        {props.title}
                        {titleIcon}
                    </span>
                </Card.Header>
                <Card.Description onClick={props.fileClicked}>
                    <div> {props.children}</div>
                    <span className="clickableChangeColor docFile">
                        {props.file ? <Icon name="file" size="big" /> : null}
                    </span>
                </Card.Description>
            </Card.Content>
            <Card.Content extra textAlign="right">
                <div>-{props.publisher}</div>
                <div>{props.date}</div>
            </Card.Content>
        </Card>
    );
};

// Feedack = Comment
export const FeedbackCardElement = (props) => {
    return (
        <Card className="FeedbackCardElement">
            <Card.Content>
                <Card.Header>
                    <div>{props.author}</div>
                </Card.Header>
                <Card.Description>
                    {props.feedback}
                    {props.icons}
                </Card.Description>
            </Card.Content>
            <Card.Content className="FeedbackCardExtra">
                <div className="FeedbackGrade">
                    Grade: {props.grade}/{props.totalGrade}
                </div>
                <div className="FeedbackDate">{props.date}</div>
            </Card.Content>
        </Card>
    );
};
