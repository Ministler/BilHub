import React from 'react';
import { Icon } from 'semantic-ui-react';

import './HomeComponents.css';

export const ProfilePrompt = (props) => {
    return (
        <div className={'ProfilePromt'}>
            <Icon name="user circle" />
            {props.name}
        </div>
    );
};

export const BriefList = (props) => {
    return (
        <div className={'BriefList'}>
            <hr />
            <div>{props.title}</div>
            {props.children}
        </div>
    );
};

export const TitledIconedBriefElement = (props) => {
    return (
        <div onClick={props.onClick}>
            {props.icon}
            {props.title}
        </div>
    );
};

export const TitledDatedBriefElement = (props) => {
    return (
        <div onClick={props.onClick}>
            <div>{props.title}</div>
            <div>{props.date}</div>
        </div>
    );
};
