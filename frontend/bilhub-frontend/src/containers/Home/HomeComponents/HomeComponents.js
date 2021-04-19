import React from 'react';
import { Icon } from 'semantic-ui-react';

import './HomeComponents.css';

export const ProfilePrompt = (props) => {
    return (
        <div>
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
        <div>
            {props.icon}
            {props.title}
        </div>
    );
};

export const TitledDatedBriefElement = (props) => {
    return (
        <div>
            <div>{props.title}</div>
            <div>{props.date}</div>
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
        <div>
            <div>
                {props.title}
                {titleIcon}
            </div>
            <div>
                {props.children}
                {props.file ? <Icon name="file" /> : null}
            </div>
            <div>{props.publisher}</div>
            <div>{props.date}</div>
        </div>
    );
};
