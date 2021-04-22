import React from 'react';
import { Icon, Container, Card } from 'semantic-ui-react';

import './HomeComponents.css';

export const ProfilePrompt = (props) => {
    return (
        <div className={'ProfilePrompt'} onClick={props.onClick}>
            <span className={'ProfileSpan'}>
                <Icon name="user circle" size="huge" />
            </span>
            <span>{props.name}</span>
        </div>
    );
};

export const BriefList = (props) => {
    return (
        <div className={'BriefList'}>
            <span className="BriefListTitle">
                <b>{props.title}</b>
            </span>
            {props.children}
        </div>
    );
};

export const TitledIconedBriefElement = (props) => {
    return (
        <div>
            <div className="BriefListElements" onClick={props.onClick}>
                <span>{props.icon}</span>
                <span>{props.title}</span>
            </div>
        </div>
    );
};

export const TitledDatedBriefElement = (props) => {
    return (
        <div>
            <span className="BriefListElements" onClick={props.onClick}>
                <div>{props.title}</div>
                <div align="right" className="DueDate">
                    {props.date}
                </div>
            </span>
        </div>
    );
};

export const FeedElement = (props) => {
    let titleIcon = null;
    if (props.status === 'graded') {
        titleIcon = <Icon name="check circle outline" />;
    } else if (props.status === 'submitted') {
        titleIcon = <Icon name="clock outline" />;
    } else if (props.status === 'notsubmitted') {
        titleIcon = <Icon name="remove circle" />;
    }
    return (
        <Card className="FeedCard">
            <Card.Content>
                <Card.Header>
                    <span className="clickable" onClick={props.titleClicked}>
                        {props.title}
                        {titleIcon}
                    </span>
                </Card.Header>
                <Card.Description onClick={props.fileClicked}>
                    <div> {props.children}</div>
                    <span className="clickable docFile">{props.file ? <Icon name="file" size="big" /> : null}</span>
                </Card.Description>
            </Card.Content>
            <Card.Content extra textAlign="right">
                <div>-{props.publisher}</div>
                <div>{props.date}</div>
            </Card.Content>
        </Card>
    );
};