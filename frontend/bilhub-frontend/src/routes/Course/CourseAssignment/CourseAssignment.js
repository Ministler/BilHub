import React, { Component } from 'react';
import { Form, Button, Grid, Modal, TextArea, Checkbox, Dropdown, Menu } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

import './CourseAssignment.css';

function RequestJoinMerge() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Request Group Join & Merge</Button>}
            size={'mini'}>
            <Modal.Header style={{ fontSize: '16px' }}>Request Group Join & Merge</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid centered>
                        <div className="ui warning message" style={{ fontSize: '12px', width: '95%' }}>
                            Please write a note and send a request
                        </div>
                        <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => setOpen(false)}
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
}

function RequestJoin() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Request Group Join</Button>}
            size={'mini'}>
            <Modal.Header style={{ fontSize: '16px' }}>Request Group Join</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid centered>
                        <div className="ui warning message" style={{ fontSize: '12px', width: '95%' }}>
                            Please write a note and send a request
                        </div>
                        <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => setOpen(false)}
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
}

function GroupReady() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Ready Status</Button>}
            style={{ width: '38%' }}>
            <Modal.Header style={{ fontSize: '16px' }}>Ready Status</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Form>
                        <Form.Field>
                            <Checkbox label="Ready 3/5" />
                            <Link
                                style={{
                                    fontSize: '14px',
                                    float: 'right',
                                }}
                                to="/notifications">
                                Chek join requests
                            </Link>
                        </Form.Field>
                        <Button
                            floated="right"
                            negative
                            onClick={() => setOpen(false)}
                            style={{
                                borderRadius: '10px',
                                padding: '5px 16px',
                                fontSize: '14px',
                                fontWeight: '500',
                                lineHeight: '20px',
                                whiteSpace: 'nowrap',
                                marginBottom: '10px',
                            }}>
                            Exit Group
                        </Button>
                    </Form>
                </Modal.Description>
            </Modal.Content>
        </Modal>
    );
}

const options = [
    { key: 1, text: 'Submission', value: 1 },
    { key: 2, text: 'Peer Review', value: 2 },
];

function AsignmentSetting() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Asignment Setting</Button>}
            size={'small'}>
            <Modal.Header style={{ fontSize: '16px' }}>Asignment Setting</Modal.Header>
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
                    onClick={() => setOpen(false)}
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
}

export class CourseAssignment extends Component {
    render() {
        return (
            <div class="ui centered grid">
                <RequestJoinMerge></RequestJoinMerge>
                <RequestJoin></RequestJoin>
                <GroupReady></GroupReady>
                <AsignmentSetting></AsignmentSetting>
            </div>
        );
    }
}
