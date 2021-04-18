import React from 'react';

import { Navbar } from '../../components';
import './Layout.css';

export const Layout = (props) => {
    return (
        <>
            <Navbar />
            <main className={'MainContent'}>{props.children}</main>
        </>
    );
};
