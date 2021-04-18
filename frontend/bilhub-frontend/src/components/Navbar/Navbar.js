import React from 'react';
import { Menu, Image, Icon } from 'semantic-ui-react';
import { NavLink } from 'react-router-dom';

import './Navbar.css';

const Navbar = (props) => {
    return (
        <Menu fixed="top" inverted size="small">
            <Menu.Item as={NavLink} to="/" exact>
                <Icon name="home" />
            </Menu.Item>

            <Menu.Item>
                <Image size="mini" src="https://react.semantic-ui.com/logo.png" /> {/* TODO: LOGOMOUZU YERLEŞTİR */}
            </Menu.Item>

            <Menu.Item as={NavLink} to="/notifications" exact position="right">
                <Icon name="bell" />
            </Menu.Item>
        </Menu>
    );
};

export { Navbar };
