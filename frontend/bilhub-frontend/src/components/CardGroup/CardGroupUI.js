import React from 'react';
import { Card } from 'semantic-ui-react';

import './CardGroupUI.css';

export const AssignmentCardElement = (props) => {
    return (
        <Card className="AssignmentCard">
            <Card.Content>
                <Card.Header>
                    <span className="clickableChangeColor" onClick={props.titleClicked}>
                        {props.title}
                        {props.titleIcon}
                    </span>
                </Card.Header>
                <Card.Description>
                    <div>{props.caption}</div>
                    <span onClick={props.fileClicked} className="clickableChangeColor docFile">
                        {props.fileIcon}
                    </span>
                </Card.Description>
            </Card.Content>
            <Card.Content extra textAlign="right">
                <div>{props.publisher}</div>
                <div>{props.date}</div>
            </Card.Content>
        </Card>
    );
};

export const FeedbackCardElement = (props) => {
    return (
        <Card className="FeedbackCardElement">
            <Card.Content>
                <Card.Header>
                    <div onClick={props.onAuthorClicked}>{props.author}</div>
                </Card.Header>
                <Card.Description>
                    {props.caption}
                    {props.icons}
                </Card.Description>
            </Card.Content>
            <Card.Content className="FeedbackCardExtra">
                <div className="FeedbackGrade">Grade: {props.grade}</div>
                <div className="FeedbackDate">{props.date}</div>
            </Card.Content>
        </Card>
    );
};

export const RequestCardElement = (props) => {
    return (
        <Card className="FeedbackCardElement">
            <Card.Content>
                <Card.Header>
                    {props.titleStart} <span onClick={props.onUserClicked}>{props.userName}</span> {props.titleMid}{' '}
                    <span onClick={props.onCourseClicked}>{props.courseName}</span>
                </Card.Header>
                <Card.Description>
                    {props.yourGroup ? (
                        <>
                            <h2>Your Group</h2>
                            {props.yourGroup}
                        </>
                    ) : null}
                    {props.otherGroup ? (
                        <>
                            <h2>Other Group</h2>
                            {props.yourGroup}
                        </>
                    ) : null}
                </Card.Description>
            </Card.Content>
            <Card.Content className="FeedbackCardExtra">
                <span>
                    {props.voteStatus}
                    {props.voteIcons}
                </span>
                {props.requestDate ? (
                    <span>
                        Request Date: {props.requestDate} / Formation Date: {props.formationDate}
                    </span>
                ) : null}
                {props.approvalDate ? <span>Approval Date: {props.approvalDate}</span> : null}
                {props.disapprovalDate ? <span>Dispproval Date: {props.disapprovalDate}</span> : null}
            </Card.Content>
        </Card>
    );
};
