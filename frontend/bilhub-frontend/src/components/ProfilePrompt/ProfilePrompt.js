import React from 'react';
import { Icon } from 'semantic-ui-react';

import './ProfilePrompt.css';

export const ProfilePrompt = (props) => {
    return (
        <div className={'ProfilePrompt'} onClick={props.onClick}>
            <span className={'ProfileSpan'}>
                <Icon name="user circle" size="huge" />
            </span>
            <span>{props.name}</span>
        </div>
    );
};
