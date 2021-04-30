import React from 'react';
import { Link } from 'react-router-dom';
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
                    {props.date}
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
        <div style={{marginTop: "10px", marginLeft: "25px"}}>
            <Link onClick={props.onSubmissionFileClicked}>
                {props.submission?.groupName} {props.submission?.fileName} &nbsp;
            </Link>
            {props.submission?.grade ? <span>Grade: {props.submission?.grade}</span> : null}
            <span style={{float:"right"}}>{props.submission?.submissionDate}</span>
        </div>
    );
};

export const GroupBriefElement = (props) => {
    return (
        <>
            {props.group?.map((member) => {
                return <p>{member}</p>;
            })}
        </>
    );
};
