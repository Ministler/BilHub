import React from 'react';
import { Form, Button, Grid, Segment } from 'semantic-ui-react';
import useSignupForm from './useSignupForm';
import './Signup.css';

const SignupUI = ({ form: { onChange, form, onSubmit, error, pepe } }) => {
    return (
        <div>
            <Grid centered>
                <Grid.Column style={{ maxWidth: 400, marginTop: 5 }}>
                    <h2 class="ui center aligned icon header">
                        <i class="circular users icon"></i>
                        Create account
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
                                <label style={{ fontSize: '12px' }}>First name</label>
                                <Form.Input
                                    type="text"
                                    name="firstName"
                                    value={form.firstName || ''}
                                    onChange={onChange}
                                />
                            </div>
                            <div class="field">
                                <label style={{ fontSize: '12px' }}>Last name</label>
                                <Form.Input
                                    type="text"
                                    name="lastName"
                                    value={form.lastName || ''}
                                    onChange={onChange}
                                />
                            </div>
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
                            <div class="field">
                                <label style={{ fontSize: '12px' }}>Re-enter password</label>
                                <Form.Input
                                    type="password"
                                    name="passwordRe"
                                    value={form.passwordRe || ''}
                                    onChange={onChange}
                                />
                            </div>
                            <Button onClick={onSubmit} fluid positive class="Sign in button" type="submit">
                                Create your BilHub account
                            </Button>
                        </Form>
                    </Segment>
                    <div class="ui center aligned segment">
                        <p
                            style={{
                                display: 'inline',
                                fontSize: '12px',
                            }}>
                            Already have an account? &nbsp;
                        </p>
                        <a
                            style={{
                                fontSize: '12px',
                            }}
                            href="/login">
                            Login here.
                        </a>
                    </div>
                </Grid.Column>
            </Grid>
        </div>
    );
};

export const Signup = (props) => {
    return <SignupUI form={useSignupForm(props)} />;
};
