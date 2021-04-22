import React from 'react';

import './ProjectComponents.css';
import { FeedList, Table } from '../../../components';
import { Accordion } from '../../../components';

export const InformationSection = (props) => {
    return (
        <div>
            <div onClick={props.onCourseClicked}>{props.courseName}</div>
            <div>
                {props.groupNameElement}
                {props.nameEditIcon}
            </div>
            <div>
                <div>Members</div>
                {props.memberElements}
            </div>
            <div>
                <div>Information</div>
                {props.informationElement}
                {props.informationEditIcon}
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

export const AssignmentPane = (props) => {
    return <FeedList>{props.feedList}</FeedList>;
};

export const GradePane = (props) => {
    return (
        <div>
            <Table bodyRowsData={props.firstBodyRowsData} headerNames={props.firstHeaderNames} />
            <Table bodyRowsData={props.secondBodyRowsData} headerNames={props.secondHeaderNames} />
            {props.finalGrade ? <div>Final Grade: {props.finalGrade}</div> : null}
        </div>
    );
};

export const FeedbackPane = (props) => {
    return (
        <>
            <Accordion accordionElements={props.accordionElements} />
            {props.newCommentButton}
        </>
    );
};
