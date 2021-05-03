import React, { Component } from 'react';

import { SignupUI } from './SignupUI';
import { ConformationUI } from './ConformationUI';
import { registerRequest, verifyRequest, resendRequest } from '../../API';

export default class Signup extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {},
            error: null,
            information: null,
            activationMode: false,
        };
    }

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

        for (let i = 0; i < this.state.form.email.length; i++)
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

        registerRequest(
            this.state.form.email,
            this.state.form.password,
            this.state.form.firstName + ' ' + this.state.form.lastName
        )
            .then((response) => {
                if (response.data.success) {
                    this.setState({
                        activationMode: true,
                        email: this.state.form.email,
                    });
                    this.setError(null);
                    this.setInformation('Please Enter the Activation Code that is Sent to Your Bilkent Email');
                }
            })
            .catch((error) => {
                this.setError('Server Error');
            });
    };

    onConformation = () => {
        verifyRequest(this.state.email, this.state.form.conformationCode)
            .then((response) => {
                this.props.history.push({
                    pathname: '/login',
                    state: { redirectedFrom: 'signup' },
                });
            })
            .catch((error) => {
                this.setInformation(null);
                this.setError('Conformation Code is Not Correct');
            });
    };

    setForm = (form) => {
        this.setState({
            form: form,
        });
    };

    setError = (error) => {
        this.setState({ error: error });
    };

    setInformation = (information) => {
        this.setState({ information: information });
    };

    onChange = (name, value) => {
        this.setForm({ ...this.state.form, [name]: value });
    };

    onResendCode = () => {
        resendRequest(this.state.email)
            .then((response) => {})
            .catch((error) => {});
    };

    render() {
        return !this.state.activationMode ? (
            <SignupUI
                onSubmit={this.onSubmit}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                error={this.state.error}
                information={this.state.information}
                onPopupClosed={() => {
                    this.setError(null);
                    this.setInformation(null);
                }}
            />
        ) : (
            <ConformationUI
                onResendCode={this.onResendCode}
                onConformation={this.onConformation}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                error={this.state.error}
                information={this.state.information}
                onPopupClosed={() => {
                    this.setError(null);
                    this.setInformation(null);
                }}
            />
        );
    }
}
