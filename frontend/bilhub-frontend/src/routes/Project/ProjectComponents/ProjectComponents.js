import React from 'react';
import { Card, Button, Icon, TextArea } from 'semantic-ui-react';

import './ProjectComponents.css';
import { Table, Accordion, Modal } from '../../../commonComponents';

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
    return <Card.Group>{props.feedList}</Card.Group>;
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
            <Accordion
                activeIndex={props.activeIndex}
                handleClick={(e, titleProps) => props.handleClick(titleProps)}
                accordionElements={props.accordionElements}
            />
            {props.newCommentButton}
        </>
    );
};

export const NewCommentModal = (props) => {
    let title = props.isTitleSRS ? 'SRS - ' : '';
    title = title + 'Feedback to ' + props.projectName + "'s Analysis Report";
    const actions = (
        <Button
            content="Give Feedback"
            labelPosition="right"
            icon="checkmark"
            onClick={() => props.closeModal(true)}
            positive
        />
    );
    return (
        <Modal isOpen={props.isOpen} closeModal={() => props.closeModal(false)} title={title} actions={actions}>
            <>
                <div>
                    <Icon name="info" />
                    Please write your feedback below
                </div>
                <TextArea onChange={(e) => props.onTextChange(e)} value={props.text} />
                <input type="file" />
                <input type="number" value={props.grade} onChange={(e) => props.onGradeChange(e)} />
            </>
        </Modal>
    );
};

export const EditCommentModal = (props) => {
    let title = props.isTitleSRS ? 'SRS - ' : '';
    title = title + 'Edit Feedback to ' + props.projectName + "'s Analysis Report";
    const actions = (
        <Button
            content="Edit Feedback"
            labelPosition="right"
            icon="edit"
            onClick={() => props.closeModal(true)}
            positive
        />
    );
    return (
        <Modal isOpen={props.isOpen} closeModal={() => props.closeModal(false)} title={title} actions={actions}>
            <>
                <div>
                    <Icon name="info" />
                    Please edit your feedback below
                </div>
                <TextArea onChange={(e) => props.onTextChange(e)} value={props.text} />
                <input type="file" />
                <input type="number" value={props.grade} onChange={(e) => props.onGradeChange(e)} />
            </>
        </Modal>
    );
};

export const DeleteCommentModal = (props) => {
    let title = props.isTitleSRS ? 'SRS - ' : '';
    title = title + 'Delete Feedback to ' + props.projectName + "'s Analysis Report";
    const actions = (
        <Button
            content="Delete Feedback"
            labelPosition="right"
            icon="checkmark"
            onClick={() => props.closeModal(true)}
            positive
        />
    );
    return (
        <Modal isOpen={props.isOpen} closeModal={() => props.closeModal(false)} title={title} actions={actions}>
            <>
                <div>
                    <Icon name="warning" />
                    Please edit your feedback below
                </div>
                <TextArea disabled value={props.text} />
                <input type="file" />
                <input disabled type="number" value={props.grade} />
            </>
        </Modal>
    );
};
