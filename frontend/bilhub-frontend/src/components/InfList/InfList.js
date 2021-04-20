import React from 'react';

import './InfList.css';

/* Expects list of divs as a prop */
export const InfList = (props) => {
    return (
        <div className={'InfDiv'}>
            <div>{props.title}</div>
            {props.children}
        </div>
    );
};
