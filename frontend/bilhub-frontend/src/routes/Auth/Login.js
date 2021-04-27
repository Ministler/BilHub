import React, { Component } from 'react';
import { connect } from 'react-redux';

import { LoginUI } from './LoginUI';
import * as actions from '../../store';
import { loginRequest } from '../../API';

class Login extends Component {
    constructor(props) {
        super(props);
        this.state = {
            form: {},
            clientError: null,
        };
    }

    setForm = (form) => {
        this.setState({
            form: form,
        });
    };

    onChange = (name, value) => {
        this.setForm({ ...this.state.form, [name]: value });
    };

    setError = (error) => {
        this.setState({ clientError: error });
    };

    onSubmit = () => {
        if (!this.state.form.email?.length || !this.state.form.password?.length) {
            this.setError('Please fill the every blank');
            return;
        }

        loginRequest(this.state.form.email, this.state.form.password).then((response) => {
            const userData = response.data;

            this.props.authSuccess(
                true,
                userData.idToken,
                userData.localId,
                userData.email,
                userData.displayName,
                'instructor'
            );
        });
    };

    render() {
        return (
            <LoginUI
                onSubmit={this.onSubmit}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                pepe={this.clearError}
                error={this.state.clientError || this.props.serverError}
            />
        );
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        authSuccess: (token, userId, email, name, userType) =>
            dispatch(actions.authSuccess(token, userId, email, name, userType)),
    };
};

export default connect(null, mapDispatchToProps)(Login);
