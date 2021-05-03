import React from 'react';
import { Icon, Button } from 'semantic-ui-react';

import { Modal } from '../../components';

export const RequestApprovalModal = (props) => {
    let title = props.requestType + ' Request Aprroval';
    const actions = (
        <Button
            content="Approve"
            labelPosition="right"
            icon="checkmark"
            onClick={() => props.closeModal(true)}
            positive
        />
    );

    return (
        <Modal isOpen={props.isOpen} closeModal={() => props.closeModal(false)} title={title} actions={actions}>
            <div>
                <Icon name="info" />
                You are about the approve {props.userName}'s {props.requestType} request!
            </div>
        </Modal>
    );
};

export const RequestDisapprovalModal = (props) => {
    let title = props.requestType + ' Request Disapproval';
    const actions = (
        <Button
            content="Disapprove"
            labelPosition="right"
            icon="checkmark"
            onClick={() => props.closeModal(true)}
            positive
        />
    );

    return (
        <Modal isOpen={props.isOpen} closeModal={() => props.closeModal(false)} title={title} actions={actions}>
            <div>
                <Icon name="info" />
                You are about the disapprove {props.userName}'s {props.requestType} request!
            </div>
        </Modal>
    );
};
