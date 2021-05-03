import React from 'react';
import { Icon, Header } from 'semantic-ui-react';

import './ProfilePrompt.css';

export const ProfilePrompt = (props) => {
    return (
        <Header as='h1' onClick={props.onClick} textAlign='center'>
            <Icon name="user circle" size="big" style={{display: "inline-block"}}/>
            <span className="clickableChangeColor">
                {props.name}
            </span>    
        </Header>
    );
};
