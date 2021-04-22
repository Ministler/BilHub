import React from 'react';
import { Modal } from 'semantic-ui-react';

export const MyModal = (props) => {
    console.log(props);
    return (
        <Modal closeIcon onClose={props.closeModal} open={props.isOpen}>
            {props.title ? <Modal.Header>{props.title}</Modal.Header> : null}
        </Modal>
    );
};
