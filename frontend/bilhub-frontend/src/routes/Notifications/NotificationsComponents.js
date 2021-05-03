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

export const RequestUndoModal = (props) => {
    let title = props.requestType + ' Request Vote Withdrawal';
    const actions = (
        <Button
            content="Withdraw Your Vote"
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
                You are about the withdraw your vote from {props.userName}'s {props.requestType} request!
            </div>
        </Modal>
    );
};

export const RequestDeleteModal = (props) => {
    let title = props.requestType + ' Request Delete';
    const actions = (
        <Button
            content="Delete Your Request"
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
                You are about the delete your request
            </div>
        </Modal>
    );
};
