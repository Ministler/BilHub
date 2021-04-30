import React, { Component } from 'react';
import { Form, Button, Modal, Checkbox } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

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
