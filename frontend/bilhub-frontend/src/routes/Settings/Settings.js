import React, { Component } from 'react';
import { Form, Icon, Segment } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

import './Settings.css';

const ProfilePrompt = (props) => {
    return (
        <div className={'ProfilePrompt'} onClick={props.onClick}>
            <span className={'ProfileSpan'}>
                <Icon name="user circle" size="huge" />
            </span>
            <Link style={{ fontSize: '24px' }} to="/profile">
                {props.name}
            </Link>
        </div>
    );
};

export class Settings extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: null,
            segment: 'Profile Settings',
            updateState: '',
        };
    }

    componentDidMount() {
        this.setState({
            user: dummyUser,
        });
    }

    changeTab = (id) => {
        if (this.state.segment !== id) this.setState({ segment: id, updateState: '' });
        else this.setState({ segment: id });
    };

    update = (e) => {
        // update data...
        this.setState({ updateState: 'Changes are successfully updated' });
    };

    pepe = (event) => {
        this.setState({ updateState: '' });
    };

    Tabs = (props) => {
        if (props.segment === 'Profile Settings') {
            return (
                <Form class="Sign in form">
                    {props.updateState && (
                        <div class="ui info message" style={{ fontSize: '12px', width: '50%' }}>
                            <i class="close icon" onClick={this.pepe}></i>
                            {props.updateState}
                        </div>
                    )}
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>First Name</label>
                        <Form.Input type="text" name="firstName" style={{ width: '60%' }} />
                    </div>
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>Last Name</label>
                        <Form.Input type="text" name="lastName" style={{ width: '60%' }} />
                    </div>
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>Bio</label>
                        <textarea rows="5" style={{ width: '60%' }}></textarea>
                    </div>
                    <button class="ui teal button" onClick={this.update}>
                        Update
                    </button>
                </Form>
            );
        } else if (props.segment === 'Account Settings') {
            return (
                <Form class="Sign in form">
                    {props.updateState && (
                        <div class="ui info message" style={{ fontSize: '12px' }}>
                            <i class="close icon" onClick={this.pepe}></i>
                            {props.updateState}
                        </div>
                    )}
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>Change Password</label>
                        <Form.Input type="password" name="password" style={{ width: '60%' }} />
                    </div>
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>Retype new Password</label>
                        <Form.Input type="password" name="passwordRe" style={{ width: '60%' }} />
                    </div>
                    <button class="ui teal button" onClick={this.update}>
                        Update
                    </button>
                </Form>
            );
        } else if (props.segment === 'Appearance') {
            return (
                <div class="ui toggle checkbox">
                    <input type="checkbox" name="public" />
                    <label>Toggle Dark</label>
                </div>
            );
        } else if (props.segment === 'Notification') {
            return <></>;
        }
    };

    render() {
        return (
            <div class="ui centered grid">
                <div class="eleven wide column" style={{ marginLeft: '50px', marginTop: '-30px' }}>
                    <div class="row">
                        <ProfilePrompt name={this.state.user?.name} onClick={this.onProfilePromptClicked} />
                    </div>
                </div>
                <div class="row">
                    <div class="three wide column">
                        <div class="ui segments">
                            <Segment
                                onClick={() => this.changeTab('Profile Settings')}
                                color={this.state.segment === 'Profile Settings' ? 'red' : ''}>
                                <p>Profile Settings</p>
                            </Segment>
                            <Segment
                                onClick={() => this.changeTab('Account Settings')}
                                color={this.state.segment === 'Account Settings' ? 'red' : ''}>
                                <p>Account Settings</p>
                            </Segment>
                            <Segment
                                onClick={() => this.changeTab('Appearance')}
                                color={this.state.segment === 'Appearance' ? 'red' : ''}>
                                <p>Appearance</p>
                            </Segment>
                            <Segment
                                onClick={() => this.changeTab('Notification')}
                                color={this.state.segment === 'Notification' ? 'red' : ''}>
                                <p>Notification</p>
                            </Segment>
                        </div>
                    </div>
                    <div class="eight wide column">
                        <p style={{ fontSize: '30px', marginTop: '-8px' }}>{this.state.segment}</p>
                        <div class="ui divider" style={{ marginTop: '-12px', width: '70%' }}></div>
                        <this.Tabs segment={this.state.segment} updateState={this.state.updateState}></this.Tabs>
                    </div>
                </div>
            </div>
        );
    }
}

const dummyUser = {
    name: 'Aybala Karakaya',
    userId: 1,
};
