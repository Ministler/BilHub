import React, { Component } from 'react';

import { convertMembersToMemberElement } from '../../../components';
import './CourseComponents.css';
import { Form, Button, Grid, TextArea, Checkbox, Modal, Dropdown, Menu } from 'semantic-ui-react';

export const InformationSection = (props) => {
    return (
        <div>
            <div>
                <h1 style={{ display: 'inline' }}>{props.courseName}</h1>
                <span className="CourseNameEdit"> {props.courseSettingsIcon} </span>
                <p>{props.description}</p>
            </div>
            <div className="InstructorsBlock">
                <h3>Instructors</h3>
                {convertMembersToMemberElement(props.instructors, props.onUserClicked)}
            </div>
            <div className="TAsBlock">
                <h3>TA's</h3>
                {convertMembersToMemberElement(props.TAs, props.onUserClicked)}
            </div>
            <div className="InformationBlock">
                <h3>Information</h3>
                <p className="InformationText" style={{ display: 'inline-block' }}>
                    {props.informationElement} &nbsp;
                </p>
                {props.informationEditIcon}
            </div>
        </div>
    );
};

const options = [
    { key: 1, text: 'Submission', value: 1 },
    { key: 2, text: 'Peer Review', value: 2 },
];

export const NewAssignmentModal = (props) => {
    return (
        <Modal closeIcon onClose={() => props.onClosed(false)} open={props.isOpen} size={'small'}>
            <Modal.Header style={{ fontSize: '16px' }}>New Assignment</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid>
                        <Form>
                            <Form.Group inline>
                                <p style={{ fontSize: '14px', float: 'left', marginBottom: '0px', display: 'inline' }}>
                                    Title
                                </p>
                                <Dropdown
                                    item
                                    simple
                                    text="Assignment Type"
                                    direction="right"
                                    options={options}
                                    style={{ marginLeft: '510px', float: 'right', display: 'inline' }}
                                />
                            </Form.Group>
                            <Form.Input
                                type="text"
                                name="groupName"
                                style={{ width: '40%', height: '35px', marginTop: '-15px' }}
                            />
                            <label style={{ fontSize: '14px', float: 'left', marginBottom: '5px' }}>Description</label>
                            <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                            <Form.Group grouped>
                                <Form.Field label="Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Anonymous Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Graded Comments" control="input" type="checkbox" />
                                <Form.Field label="Groups' Submission Visable" control="input" type="checkbox" />
                                <Form.Field label="Late Submission" control="input" type="checkbox" />
                                <Form.Field label="Due Date" />
                                <Form.Field label="Add File" />
                            </Form.Group>
                        </Form>
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => props.onClosed(false)}
                    style={{
                        borderRadius: '10px',
                        padding: '5px 16px',
                        fontSize: '14px',
                        fontWeight: '500',
                        lineHeight: '20px',
                        whiteSpace: 'nowrap',
                    }}>
                    Send
                </Button>
            </Modal.Actions>
        </Modal>
    );
};

export const EditAssignmentModal = (props) => {
    return (
        <Modal closeIcon onClose={() => props.onClosed(false)} open={props.isOpen} size={'small'}>
            <Modal.Header style={{ fontSize: '16px' }}>New Assignment</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid>
                        <Form>
                            <Form.Group inline>
                                <p style={{ fontSize: '14px', float: 'left', marginBottom: '0px', display: 'inline' }}>
                                    Title
                                </p>
                                <Dropdown
                                    item
                                    simple
                                    text="Assignment Type"
                                    direction="right"
                                    options={options}
                                    style={{ marginLeft: '510px', float: 'right', display: 'inline' }}
                                />
                            </Form.Group>
                            <Form.Input
                                type="text"
                                name="groupName"
                                style={{ width: '40%', height: '35px', marginTop: '-15px' }}
                            />
                            <label style={{ fontSize: '14px', float: 'left', marginBottom: '5px' }}>Description</label>
                            <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                            <Form.Group grouped>
                                <Form.Field label="Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Anonymous Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Graded Comments" control="input" type="checkbox" />
                                <Form.Field label="Groups' Submission Visable" control="input" type="checkbox" />
                                <Form.Field label="Late Submission" control="input" type="checkbox" />
                                <Form.Field label="Due Date" />
                                <Form.Field label="Add File" />
                            </Form.Group>
                        </Form>
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => props.onClosed(false)}
                    style={{
                        borderRadius: '10px',
                        padding: '5px 16px',
                        fontSize: '14px',
                        fontWeight: '500',
                        lineHeight: '20px',
                        whiteSpace: 'nowrap',
                    }}>
                    Send
                </Button>
            </Modal.Actions>
        </Modal>
    );
};

export const DeleteAssignmentModal = (props) => {
    return (
        <Modal closeIcon onClose={() => props.onClosed(false)} open={props.isOpen} size={'small'}>
            <Modal.Header style={{ fontSize: '16px' }}>New Assignment</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid>
                        <Form>
                            <Form.Group inline>
                                <p style={{ fontSize: '14px', float: 'left', marginBottom: '0px', display: 'inline' }}>
                                    Title
                                </p>
                                <Dropdown
                                    item
                                    simple
                                    text="Assignment Type"
                                    direction="right"
                                    options={options}
                                    style={{ marginLeft: '510px', float: 'right', display: 'inline' }}
                                />
                            </Form.Group>
                            <Form.Input
                                type="text"
                                name="groupName"
                                style={{ width: '40%', height: '35px', marginTop: '-15px' }}
                            />
                            <label style={{ fontSize: '14px', float: 'left', marginBottom: '5px' }}>Description</label>
                            <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                            <Form.Group grouped>
                                <Form.Field label="Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Anonymous Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Graded Comments" control="input" type="checkbox" />
                                <Form.Field label="Groups' Submission Visable" control="input" type="checkbox" />
                                <Form.Field label="Late Submission" control="input" type="checkbox" />
                                <Form.Field label="Due Date" />
                                <Form.Field label="Add File" />
                            </Form.Group>
                        </Form>
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => props.onClosed(false)}
                    style={{
                        borderRadius: '10px',
                        padding: '5px 16px',
                        fontSize: '14px',
                        fontWeight: '500',
                        lineHeight: '20px',
                        whiteSpace: 'nowrap',
                    }}>
                    Send
                </Button>
            </Modal.Actions>
        </Modal>
    );
};
