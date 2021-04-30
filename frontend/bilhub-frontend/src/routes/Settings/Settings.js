import React, { Component } from 'react';
import { Form, Icon, Segment } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import * as actions from '../../store';

import './Settings.css';

class Settings extends Component {
    constructor(props) {
        super(props);
        this.state = {
            segment: 'Profile Settings',
            error: '',
            information: '',
            userInformation: '',

            email: '',
            verifyCode: '',
            sendConformation: false,
        };
    }

    updateProfile = (e) => {
        const newName = e.target.name?.value;
        if (newName.length === 0) {
            this.setInformation(null);
            this.setError('Please fill all areas');
            return;
        }

        const request = {
            newName: e.target.name?.value,
            newInformation: e.target.information?.value,
            userId: this.props.userId,
        };
        console.log(request);
    };

    updatePassword = (e) => {
        const newPassword = e.target.password?.value;
        const reNewPassword = e.target.passwordRe?.value;
        if (newPassword !== reNewPassword) {
            this.setInformation(null);
            this.setError('Password do not match');
            return;
        }

        const request = {
            newPassword: e.target.password?.value,
            userId: this.props.userId,
        };
        console.log(request);
    };

    updateEmail = () => {
        const newEmail = this.state.email;

        for (let i = 0; i < newEmail.length; i++)
            if (newEmail[i] === '@' && i + 1 < newEmail.length && newEmail.indexOf('bilkent', i + 1) === -1) {
                this.setError('Please use your bilkent email');
                return;
            }

        const request = {
            newEmail: newEmail,
            userId: this.props.userId,
        };

        if (true) {
            this.setState({
                sendConformation: true,
            });
        } else {
            this.setError('Something wrong');
        }

        console.log(request);
    };

    onVerifyEmail = () => {
        const code = this.state.code;

        const request = {
            code: code,
            userId: this.props.userId,
        };

        if (true) {
            this.setState({
                sendConformation: false,
            });
            this.props.logout();
        } else {
            this.setError('Conformation Code is Wrong');
        }

        console.log(request);
    };

    updateDarkMode = (e) => {
        const request = {
            newDarKMode: e.target.checked,
            userId: this.props.userId,
        };
        console.log(request);
    };

    componentDidMount() {
        this.setState({
            userInformation: dummyUser.userInformation,
        });
    }

    changeTab = (id) => {
        if (this.state.segment !== id) this.setState({ segment: id, error: null, information: null });
        else this.setState({ segment: id });
    };

    setError = (error) => {
        this.setState({ error: error });
    };

    setInformation = (information) => {
        this.setState({ information: information });
    };

    onErrorClosed = (event) => {
        this.setState({ error: '' });
    };

    onInputChange = (event, name, value) => {
        this.setState({
            [name]: value,
        });
    };

    sendConformation = (event) => {
        this.setState({ sendConformation: true });
    };

    Tabs = (props) => {
        if (props.segment === 'Profile Settings') {
            return (
                <Form class="Sign in form" onSubmit={this.updateProfile}>
                    {props.error && (
                        <div class="ui negative message" style={{ fontSize: '12px', width: '60%' }}>
                            <i class="close icon" onClick={() => this.setError(null)}></i>
                            {props.error}
                        </div>
                    )}
                    {props.information && (
                        <div class="ui info message" style={{ fontSize: '12px', width: '60%' }}>
                            <i class="close icon" onClick={() => this.setInformation(null)}></i>
                            {props.information}
                        </div>
                    )}
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>Name</label>
                        <Form.Input
                            type="text"
                            defaultValue={this.props.userName}
                            name="name"
                            style={{ width: '60%' }}
                        />
                    </div>
                    <div class="field">
                        <label style={{ fontSize: '12px' }}>Bio</label>
                        <textarea
                            rows="5"
                            defaultValue={this.state.userInformation}
                            name="information"
                            style={{ width: '60%' }}></textarea>
                    </div>
                    <button class="ui primary button">Update</button>
                </Form>
            );
        } else if (props.segment === 'Account Settings') {
            return (
                <>
                    {props.error && (
                        <div class="ui negative message" style={{ fontSize: '12px', width: '60%' }}>
                            <i class="close icon" onClick={() => this.setError(null)}></i>
                            {props.error}
                        </div>
                    )}
                    {props.information && (
                        <div class="ui info message" style={{ fontSize: '12px', width: '60%' }}>
                            <i class="close icon" onClick={() => this.setInformation(null)}></i>
                            {props.information}
                        </div>
                    )}
                    <Form class="Sign in form" onSubmit={this.updatePassword}>
                        <div class="field">
                            <label style={{ fontSize: '12px' }}>Change Bilkent Email</label>
                            <Form.Input
                                type="email"
                                name="email"
                                disabled={this.state.sendConformation}
                                style={{ width: '60%' }}
                                onChange={(e, { name, value }) => this.onInputChange(e, name, value)}
                                action={{
                                    content: 'Change',
                                    onClick: () => this.updateEmail(),
                                }}
                            />
                        </div>
                        {props.sendConformation && (
                            <div class="field">
                                <label style={{ fontSize: '12px' }}>Conformation Code</label>
                                <Form.Input
                                    type="text"
                                    name="code"
                                    style={{ width: '60%' }}
                                    onChange={(e, { name, value }) => this.onInputChange(e, name, value)}
                                />
                            </div>
                        )}
                        {props.sendConformation && (
                            <button
                                class="ui primary button"
                                onClick={this.onVerifyEmail}
                                style={{ marginBottom: '10px' }}>
                                Update
                            </button>
                        )}

                        <div class="field">
                            <label style={{ fontSize: '12px' }}>Change Password</label>
                            <Form.Input type="password" name="password" style={{ width: '60%' }} />
                        </div>
                        <div class="field">
                            <label style={{ fontSize: '12px' }}>Retype New Password</label>
                            <Form.Input type="password" name="passwordRe" style={{ width: '60%' }} />
                        </div>
                        <button class="ui primary button">Update Password</button>
                    </Form>
                </>
            );
        } else if (props.segment === 'Appearance') {
            return (
                <div class="ui toggle checkbox">
                    <input
                        type="checkbox"
                        name="darkmode"
                        defaultChecked={this.props.darkMode}
                        onChange={this.updateDarkMode}
                    />
                    <label>Dark Mode</label>
                </div>
            );
        }
        // } else if (props.segment === 'Notification') {
        //     return <></>;
        // }
    };

    render() {
        return (
            <div class="ui centered grid">
                <div class="row" style={{ marginTop: '-30px' }}>
                    <div class="three wide column">
                        <div style={{ textAlign: 'center' }}>
                            <ProfilePrompt name={this.props.userName} onClick={this.onProfilePromptClicked} />
                        </div>
                    </div>
                    <div class="eight wide column"></div>
                </div>
                <div class="row" style={{ marginTop: '-10px' }}>
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
                            {/* <Segment
                                onClick={() => this.changeTab('Notification')}
                                color={this.state.segment === 'Notification' ? 'red' : ''}>
                                <p>Notification</p>
                            </Segment> */}
                        </div>
                    </div>
                    <div class="eight wide column">
                        <p style={{ fontSize: '30px', marginTop: '-8px' }}>{this.state.segment}</p>
                        <div class="ui divider" style={{ marginTop: '-12px', width: '70%' }}></div>
                        <this.Tabs
                            segment={this.state.segment}
                            error={this.state.error}
                            information={this.state.information}
                            sendConformation={this.state.sendConformation}></this.Tabs>
                    </div>
                </div>
            </div>
        );
    }
}

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

const mapStateToProps = (state) => {
    return {
        userName: state.name,
        userId: state.userId,
        darkMode: state.darkMode,
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        logout: () => dispatch(actions.logout()),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Settings);

const dummyUser = {
    userInformation:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Delectus id aspernatur ea sitanimi, ab qui! Ea beatae dolorum inventore cum quibusdam placeat quisquam itaque, odioquasi numquam maiores quidem illum odit commodi dicta animi voluptas tempora? Adipisci maiores inventore minus provident quas minima itaque saepe et labore, ut sequi!',
};
