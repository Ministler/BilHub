import React from 'react';

import { Navbar } from '../../Navbar';
import './AppLayout.css';

export const AppLayout = (props) => {
    return (
        <>
            <Navbar />
            <main className={'AppLayoutMain'}>{props.children}</main>
        </>
    );
};
