import React, { Component } from 'react';
import { connect } from 'react-redux';

import { LoginUI } from './LoginUI';
import * as actions from '../../store';

class Login extends Component {
    constructor(props) {
        super(props);
        this.props.resetSignupSucceed();
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

        this.props.onLogin(this.state.form.email, this.state.form.password);
    };

    render() {
        return (
            <LoginUI
                onSubmit={this.onSubmit}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                errorCloseButton={this.clearError}
                error={this.state.clientError || this.props.serverError}
            />
        );
    }
}

const mapStateToProps = (state) => {
    return {
        serverError: state.loginError,
        loadign: state.loginLoading,
        redirectFromSignup: state.redirectFromSignup,
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        resetSignupSucceed: () => dispatch(actions.resetSignupSucceed()),
        onLogin: (email, password) => dispatch(actions.login(email, password)),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Login);
