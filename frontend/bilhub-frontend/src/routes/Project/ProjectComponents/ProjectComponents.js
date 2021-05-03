import React from 'react';
import { Button, Icon, TextArea, Form, Grid, Input } from 'semantic-ui-react';

import './ProjectComponents.css';
import { Modal } from '../../../components';

export const InformationSection = (props) => {
    return (
        <div>
            <div style={{ textAlign: 'center' }}>
                <div className="clickableChangeColor" onClick={props.onCourseClicked}>
                    <h1>{props.courseName}</h1>
                </div>
                <h2 className="ProjectName" style={{ display: 'inline' }}>
                    {props.groupNameElement}
                </h2>
                <span className="GroupNameEdit"> {props.nameEditIcon} </span>
            </div>
            <div className="MembersBlock">
                <div>
                    <h4 style={{ marginLeft: '20px' }}>Members</h4>
                </div>
                {props.memberElements}
            </div>
            <div className="InformationBlock">
                <div>
                    <h4 style={{ marginLeft: '20px', marginBottom: '10px' }}>Information</h4>
                </div>
                <p className="InformationText" style={{ display: 'inline-block' }}>
                    {props.informationElement} &nbsp;
                </p>
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
                <TextArea className="FeedbackText" onChange={(e) => props.onTextChange(e)} value={props.text} />
                <input type="file" />
                {!props.isTitleSRS ? (
                    <span style={{ float: 'right' }}>
                        <input
                            type="number"
                            max="10"
                            min="0"
                            value={props.grade}
                            onChange={(e) => props.onGradeChange(e)}
                        />{' '}
                        out of 10
                    </span>
                ) : (
                    <span style={{ float: 'right' }}>
                        <input
                            type="number"
                            style={{ width: '30px' }}
                            value={props.grade}
                            onChange={(e) => props.onGradeChange(e)}
                        />{' '}
                        out of{' '}
                        <input
                            type="number"
                            style={{ width: '30px' }}
                            value={props.maxGrade}
                            onChange={(e) => props.onMaxGradeChange(e)}
                        />
                    </span>
                )}
            </>
        </Modal>
    );
};

export const NewCommentModal2 = (props) => {
    return (
        <div class="sixteen wide column">
            <p>Your Feedback:</p>
            <Form reply style={{ width: '95%' }}>
                <Form.TextArea rows="5" onChange={(e) => props.onTextChange(e)} value={props.text} />
            </Form>
            <Grid style={{ marginTop: '10px', width: '98%' }}>
                <Grid.Row columns={2}>
                    <Grid.Column width={9}>
                        <p style={{ display: 'inline' }}>Grade &nbsp;</p>
                        <Input
                            type="number"
                            content="Grade Placeholder"
                            max={props.maxGrade}
                            min="0"
                            floated="left"
                            type="number"
                            value={props.grade}
                            onChange={(e) => props.onGradeChange(e)}
                        />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="file" onChange={(e) => this.props.onFileChanged(e.target.files[0])} />
                    </Grid.Column>
                    <Grid.Column width={7}>
                        <Button
                            content="Give Feedback"
                            primary
                            Compact
                            floated="right"
                            labelPosition="right"
                            icon="plus"
                            onClick={(e) => props.onGiveFeedback(e)}
                        />
                    </Grid.Column>
                </Grid.Row>
            </Grid>
        </div>
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
                {!props.isTitleSRS ? (
                    <>
                        <input
                            type="number"
                            max="10"
                            min="0"
                            value={props.grade}
                            onChange={(e) => props.onGradeChange(e)}
                        />{' '}
                        out of 10
                    </>
                ) : (
                    <>
                        <input type="number" value={props.grade} onChange={(e) => props.onGradeChange(e)} /> out of{' '}
                        <input type="number" value={props.maxGrade} onChange={(e) => props.onMaxGradeChange(e)} />
                    </>
                )}
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
                <input disabled type="number" value={props.grade} /> out of {props.maxGrade ? props.maxGrade : '10'}
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

export const NewSubmissionModal2 = (props) => {
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
