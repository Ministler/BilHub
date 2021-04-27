import React from 'react';

import { convertMembersToMemberElement } from '../../../components';

import { Button, Modal, Icon, TextArea } from 'semantic-ui-react';

export const InformationSection = (props) => {
    return (
        <div>
            <div>
                <h1>
                    {props.courseName}
                    {props.courseSettingsIcon}
                </h1>

                <h3>{props.description}</h3>
            </div>
            <div>
                <h2>Instructor</h2>
                {convertMembersToMemberElement(props.instructors, props.onUserClicked)}
            </div>
            <div>
                <h2>TA's</h2>
                {convertMembersToMemberElement(props.TAs, props.onUserClicked)}
            </div>
            <div>
                <h3>Information</h3>
                {props.informationEditIcon}
                <p>{props.informationElement}</p>
            </div>
        </div>
    );
};

export const FormationGroupModal = (props) => {
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

export const FormedGroupModal = (props) => {
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
