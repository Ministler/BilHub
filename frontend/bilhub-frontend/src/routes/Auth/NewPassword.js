import React, { Component } from 'react';
import { NewPasswordUI } from './NewPasswordUI';

export default class NewPassword extends Component {
    constructor(props) {
        super(props);

        this.state = {
            form: {},
            error: null,
        };
    }

    onSubmit = () => {
        if (!this.state.form.email?.length) {
            this.setError('Please fill the every blank');
            return;
        }

        if (true) {
            if (true) {
                this.props.history.push({
                    pathname: '/login',
                    state: { redirectedFrom: 'newPassword' },
                });
            } else {
                this.setError('No Such Mail');
            }
        }
    };

    onChange = (name, value) => {
        this.setForm({ ...this.state.form, [name]: value });
    };

    setForm = (form) => {
        this.setState({
            form: form,
        });
    };

    setError = (error) => {
        this.setState({ error: error });
    };

    render() {
        return (
            <NewPasswordUI
                onSubmit={this.onSubmit}
                onChange={(e, { name, value }) => this.onChange(name, value)}
                form={this.state.form}
                onErrorClosed={() => this.setError(null)}
                error={this.state.error}></NewPasswordUI>
        );
    }
}
