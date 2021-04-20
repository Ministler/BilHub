import React from 'react';

import './ProjectComponents.css';

export const InformationSection = (props) => {
    return (
        <div>
            <div onClick={props.onCourseClicked}>{props.group.courseName}</div>
            <div>{props.group.name}</div>
            <div>
                <div>Members</div>
                {props.memberElements}
            </div>
            <div>
                <div>Information</div>
                {props.group.information}
            </div>
        </div>
    );
};

export const MemberElement = (props) => {
    return (
        <div onClick={props.onClick}>
            {props.member.name} - {props.member.information}
        </div>
    );
};
