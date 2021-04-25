import React from 'react';
import { Modal } from 'semantic-ui-react';

import './Modal.css';

export const MyModal = (props) => {
    return (
        <Modal closeIcon onClose={props.closeModal} open={props.isOpen}>
            {props.title ? <Modal.Header>{props.title}</Modal.Header> : null}
            {props.children ? <Modal.Content>{props.children}</Modal.Content> : null}
            {props.actions ? <Modal.Actions>{props.actions}</Modal.Actions> : null}
        </Modal>
    );
};
