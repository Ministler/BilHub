import React, { useState } from 'react';
import { Form, Button, Grid, Header, Segment, Message } from 'semantic-ui-react';
import useLoginForm from './useLoginForm';
import './Login.css';

const LoginUI = ({ form: { onChange, form, onSubmit, error, pepe } }) => {
    return (
        <div>
            <Grid centered>
                <Grid.Column style={{ maxWidth: 300, marginTop: 50 }}>
                    <h2 class="ui center aligned icon header">
                        <i class="circular users icon"></i>
                        Sign in to BilHub
                    </h2>
                    {error && (
                        <div class="ui negative message" style={{ fontSize: '12px' }}>
                            <i class="close icon" onClick={pepe}></i>
                            {error}
                        </div>
                    )}
                    <Segment>
                        <Form class="Sign in form">
                            <div class="field">
                                <label style={{ fontSize: '12px' }}>Bilkent email adress</label>
                                <Form.Input type="email" name="email" value={form.email || ''} onChange={onChange} />
                            </div>
                            <div class="field">
                                <label style={{ fontSize: '12px' }}>Password</label>
                                <Form.Input
                                    type="password"
                                    name="password"
                                    value={form.password || ''}
                                    onChange={onChange}
                                />
                            </div>
                            <Button onClick={onSubmit} fluid positive class="Sign in button" type="submit">
                                Login
                            </Button>
                        </Form>
                    </Segment>
                    <div class="ui center aligned segment">
                        <p
                            style={{
                                display: 'inline',
                                fontSize: '12px',
                            }}>
                            New to BilHub? &nbsp;
                        </p>
                        <a
                            style={{
                                fontSize: '12px',
                            }}
                            href="/signup">
                            Create an account.
                        </a>
                    </div>
                </Grid.Column>
            </Grid>
        </div>
    );
};

export const Login = (props) => {
    return <LoginUI form={useLoginForm(props)} />;
};
