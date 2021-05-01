import React, { Component } from 'react';
import { connect } from 'react-redux';

import { LoginUI } from './LoginUI';
import * as actions from '../../store';
import { loginRequest } from '../../API';

class Login extends Component {
    constructor(props) {
        super(props);

        let information = null;
        if (this.props?.location?.state?.redirectedFrom === 'signup') {
            information = 'Your Account Created';
        }
        if (this.props?.location?.state?.redirectedFrom === 'newPassword') {
            information = 'Your New Password been sent your Email';
        }

        this.state = {
            form: {},
            error: null,
            information: information,
        };
    }

    onSubmit = () => {
        if (!this.state.form.email?.length || !this.state.form.password?.length) {
            this.setError('Please fill the every blank');
            return;
        }

        loginRequest(this.state.form.email, this.state.form.password)
            .then((response) => {
                const userData = response.data.data;
                console.log(userData);
                this.props.authSuccess(
                    userData.token,
                    userData.id,
                    userData.email,
                    userData.name,
                    userData.userType,
                    userData.darkModeStatus
                );
            })
            .catch(() => {
                this.setInformation(null);
                this.setError('Server Error');
            });
    };

    setForm = (form) => {
        this.setState({
            form: form,
        });
    };

    onChange = (name, value) => {
        this.setForm({ ...this.state.form, [name]: value });
    };

    setError = (error) => {
        this.setState({ error: error });
    };

    setInformation = (information) => {
        this.setState({ information: information });
    };

    render() {
        return (
            <LoginUI
                onSubmit={this.onSubmit}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                onPopupClosed={() => {
                    this.setError(null);
                    this.setInformation(null);
                }}
                error={this.state.error}
                information={this.state.information}
            />
        );
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        authSuccess: (token, userId, email, name, userType, darkMode) =>
            dispatch(actions.authSuccess(token, userId, email, name, userType, darkMode)),
    };
};

export default connect(null, mapDispatchToProps)(Login);
