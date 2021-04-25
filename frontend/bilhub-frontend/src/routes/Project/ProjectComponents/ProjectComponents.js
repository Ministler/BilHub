import React from 'react';
import { Button, Icon, TextArea } from 'semantic-ui-react';

import './ProjectComponents.css';
import { Modal, AssignmentCardElement } from '../../../components';

export const InformationSection = (props) => {
    return (
        <div>
            <div className="clickableChangeColor" onClick={props.onCourseClicked}>
                <h1>{props.courseName}</h1>
            </div>
            <div>
                <h1>
                    {props.groupNameElement}
                    <span className="GroupNameEdit"> {props.nameEditIcon} </span>
                </h1>
            </div>
            <div className="MembersBlock">
                <div>
                    <h3>Members</h3>
                </div>
                {props.memberElements}
            </div>
            <div className="InformationBlock">
                <div>
                    <h3>Information</h3>
                </div>
                <p className="InformationText">{props.informationElement}</p>
                {props.informationEditIcon}
            </div>
        </div>
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

export const NewSubmissionModal = (props) => {
    const title = 'Add Submission for ' + props.assignmentName;
    const actions = (
        <Button
            content="Add Submission"
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
                    {props.instructions}
                </div>
                <TextArea onChange={(e) => props.onTextChange(e)} value={props.text} />
                <input type="file" />
            </>
        </Modal>
    );
};

export const EditSubmissionModal = (props) => {
    const title = 'Edit Submission for ' + props.assignmentName;
    const actions = (
        <Button
            content="Edit Submission"
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
                    {props.instructions}
                </div>
                <TextArea onChange={(e) => props.onTextChange(e)} value={props.text} />
                <input type="file" />
            </>
        </Modal>
    );
};

export const DeleteSubmissionModal = (props) => {
    const title = 'Delete Submission of ' + props.assignmentName;
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
                    "You are about the delete your file."
                </div>
                <TextArea disabled value={props.text} />
                <input type="file" />
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
