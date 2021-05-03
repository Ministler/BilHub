import React, { Component } from 'react';
import { Button, Grid, Modal, TextArea } from 'semantic-ui-react';

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
