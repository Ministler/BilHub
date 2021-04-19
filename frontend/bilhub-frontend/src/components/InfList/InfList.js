import React from 'react';

import './InfList.css';

/* Expects list of divs as a prop */
export const InfList = (props) => {
    return <div className={'InfDiv'}>{props.children}</div>;
};
