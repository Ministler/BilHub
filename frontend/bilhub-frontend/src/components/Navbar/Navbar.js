import React from 'react';
import { Search, Menu, Image, Icon, Dropdown } from 'semantic-ui-react';
import { NavLink } from 'react-router-dom';
import { connect } from 'react-redux';

import './Navbar.css';
import * as actions from '../../store';

const Navbar = (props) => {
    return (
        <Menu className="NavMenu" fixed="top" inverted size="small">
            <Menu.Item as={NavLink} to="/" exact>
                <Image size="mini" src="https://react.semantic-ui.com/logo.png" /> {/* TODO: LOGOMOUZU YERLEŞTİR */}
            </Menu.Item>

            {/* <Menu.Item>
                <Search />
            </Menu.Item> */}
            <Menu.Menu className="NavigatorRightMenu" position="right">
                <Menu.Item className="NotificationNavigate" as={NavLink} to="/notifications" exact position="right">
                    <Icon name="bell" size="large" />
                </Menu.Item>
                <Dropdown icon={{ name: 'user circle outline', size: 'big' }} simple item closeOnChange={true}>
                    <Dropdown.Menu direction="left">
                        <div className="MyDivider" />
                        <Dropdown.Item as={NavLink} to="/profile" icon="user circle outline" text="My Profile" />

                        {props.userType === 'Instructor' ? (
                            <>
                                <Dropdown.Divider />
                                <Dropdown.Item
                                    as={NavLink}
                                    to="/create-new-course"
                                    icon="star outline"
                                    text="Create New Course"
                                />
                                <Dropdown.Divider />
                            </>
                        ) : (
                            <Dropdown.Divider />
                        )}

                        <Dropdown.Item as={NavLink} to="/settings" icon="settings" text="Settings" />
                        <Dropdown.Divider />
                        <Dropdown.Item onClick={props.logout} icon="sign out" text="Sign Out" />
                        <div className="MyDivider" />
                    </Dropdown.Menu>
                </Dropdown>
            </Menu.Menu>
        </Menu>
    );
};

const mapStateToProps = (state) => {
    return {
        userType: state.userType,
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        logout: () => dispatch(actions.logout()),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Navbar);
