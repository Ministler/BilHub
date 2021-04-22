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
