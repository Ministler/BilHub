import React from 'react';

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
            {props.member.name} - {props.member.information}
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
