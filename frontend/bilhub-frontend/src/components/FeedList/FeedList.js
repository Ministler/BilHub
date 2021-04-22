import React from 'react';
import { Icon } from 'semantic-ui-react';

import './FeedList.css';

/* Expects list of divs as a prop */
export const FeedList = (props) => {
    return (
        <div className={'FeedListDiv'}>
            {props.title ? <div>{props.title}</div> : null}
            {props.children}
        </div>
    );
};

// Feedack = Comment
export const FeedbackFeedElement = (props) => {
    return (
        <div>
            <div>{props.feedback}</div>
            <div>
                <span>
                    Grade: {props.grade}/{props.totalGrade}
                </span>
                <span>
                    {props.icons}
                    {props.author}, {props.date}
                </span>
            </div>
        </div>
    );
};

export const AssignmentFeedElement = (props) => {
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
            <div onClick={props.titleClicked}>
                {props.title}
                {titleIcon}
            </div>
            <div onClick={props.fileClicked}>
                {props.children}
                {props.file ? <Icon name="file" /> : null}
            </div>
            <div onClick={props.publisherClicked}>{props.publisher}</div>
            <div>{props.date}</div>
        </div>
    );
};
