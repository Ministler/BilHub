import React, { Component } from 'react';

import { SignupUI } from './SignupUI';
import { singupRequest } from '../../API';

export default class Signup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {},
            error: null,
        };
    }

    setForm = (form) => {
        this.setState({
            form: form,
        });
    };

    setError = (error) => {
        this.setState({ error: error });
    };

    onChange = (name, value) => {
        this.setForm({ ...this.state.form, [name]: value });
    };

    onSubmit = () => {
        if (
            !this.state.form.firstName?.length ||
            !this.state.form.lastName?.length ||
            !this.state.form.email?.length ||
            !this.state.form.password?.length ||
            !this.state.form.passwordRe?.length
        ) {
            this.setError('Please fill the every blank');
            return;
        }

        for (var i = 0; i < this.state.form.email.length; i++)
            if (
                this.state.form.email[i] === '@' &&
                i + 1 < this.state.form.email.length &&
                this.state.form.email.indexOf('bilkent', i + 1) === -1
            ) {
                this.setError('Please use your bilkent email');
                return;
            }

        if (this.state.form.password !== this.state.form.passwordRe) {
            this.setError('Passwords dont match');
            return;
        }

        singupRequest(
            this.state.form.email,
            this.state.form.password,
            this.state.form.firstName + this.state.form.lastName
        ).then((response) => {
            this.props.history.push('/login');
        });
    };

    render() {
        if (this.props.requestFullfilled) {
            this.props.history.push('/login');
        }

        return (
            <SignupUI
                onSubmit={this.onSubmit}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                error={this.state.clientError || this.props.serverError}
            />
        );
    }
}
