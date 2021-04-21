import React from 'react';
import { Search, Menu, Image, Icon, Dropdown } from 'semantic-ui-react';
import { NavLink } from 'react-router-dom';
import { ProfileNav } from './ProfileNav';

import './Navbar.css';

const Navbar = (props) => {
    return (
        <Menu className="NavMenu" fixed="top" inverted size="small">
            <Menu.Item as={NavLink} to="/" exact>
                <Image size="mini" src="https://react.semantic-ui.com/logo.png" /> {/* TODO: LOGOMOUZU YERLEŞTİR */}
            </Menu.Item>

            <Menu.Item>
                <Search />
            </Menu.Item>
            <Menu.Menu position="right">
                <Dropdown as={Menu.Item} icon={{ name: 'user circle outline', size: 'big' }} className="ProfileNav">
                    <Dropdown.Menu direction="left">
                        <Dropdown.Item as={NavLink} to="/profile" icon="user circle outline" text="My Profile" />
                        <Dropdown.Divider />
                        <Dropdown.Item as={NavLink} to="/profile" icon="star outline" text="Create New Class" />
                        <Dropdown.Divider />
                        <Dropdown.Item as={NavLink} to="/settings" icon="settings" text="Settings" />
                        <Dropdown.Divider />
                        <Dropdown.Item as={NavLink} to="/logout" icon="sign out" text="Sign Out" />
                    </Dropdown.Menu>
                </Dropdown>

                <Menu.Item className="NotificationNavigate" as={NavLink} to="/notifications" exact position="right">
                    <Icon name="bell" size="large" />
                </Menu.Item>
            </Menu.Menu>
        </Menu>
    );
};

export { Navbar };
