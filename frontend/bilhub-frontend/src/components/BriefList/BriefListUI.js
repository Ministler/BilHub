import React from 'react';
import { Link } from 'react-router-dom';
import { Icon } from 'semantic-ui-react';
import { dateObjectToString } from '../../utils';

import './BriefListUI.css';

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
            <div className="BriefListElements clickableHighlightBack" onClick={props.onClick}>
                <span>{props.icon}</span>
                <span>{props.title}</span>
            </div>
        </div>
    );
};

export const TitledDatedBriefElement = (props) => {
    return (
        <div>
            <span className="BriefListElements clickableHighlightBack" onClick={props.onClick}>
                <div>{props.title}</div>
                <div align="right" className="DueDate">
                    {typeof props.date === 'object' ? dateObjectToString(props.date) : props.date}
                </div>
            </span>
        </div>
    );
};

export const MemberBriefElement = (props) => {
    return (
        <div className="clickableHighlightBack" onClick={props.onClick}>
            {props.member.name}
        </div>
    );
};

export const SubmissionBriefElement = (props) => {
    return (
        <div style={{ marginTop: '10px', marginLeft: '25px' }}>
            <span onClick={props.onSubmissionPageClicked} style={{ fontWeight: 'bold' }}>
                {props.submission?.groupName}: &nbsp;
            </span>
            <Link onClick={props.onSubmissionPageClicked}>{props.submission?.fileName} &nbsp;</Link>
            {props.submission?.hasFile ? (
                <Icon name="file" onClick={props.onSubmissionFileClicked} color="grey" />
            ) : null}
            {props.submission?.grade ? <span>Grade: {props.submission?.grade}</span> : null}
            <span style={{ float: 'right' }}>
                {typeof props.submission?.submissionDate === 'object'
                    ? dateObjectToString(props.submission?.submissionDate)
                    : props.submission?.submissionDate}
            </span>
        </div>
    );
};

export const GroupBriefElement = (props) => {
    return (
        <>
            {props.group?.map((member) => {
                return <p>{member.name}</p>;
            })}
        </>
    );
};
