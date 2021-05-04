import React from 'react';
import { Form, Button, Grid, Segment, Image } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import logo from '../../assets/logo.png';

import './SignupUI.css';

export const SignupUI = (props) => {
    return (
        <div>
            <Grid centered>
                <Grid.Column style={{ maxWidth: 400, marginTop: 5 }}>
                    <Image src={logo} size="small" centered></Image>
                    <h2 className="ui center aligned icon header">Create account</h2>
                    {props.error && (
                        <div className="ui negative message" style={{ fontSize: '12px' }}>
                            <i className="close icon" onClick={props.onPopupClosed}></i>
                            {props.error}
                        </div>
                    )}
                    {props.information && (
                        <div className="ui postive message" style={{ fontSize: '12px' }}>
                            <i className="close icon" onClick={props.onPopupClosed}></i>
                            {props.information}
                        </div>
                    )}
                    <Segment>
                        <Form className="Sign in form">
                            <div className="field">
                                <label style={{ fontSize: '12px' }}>First name</label>
                                <Form.Input
                                    type="text"
                                    name="firstName"
                                    value={props.form.firstName || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <div className="field">
                                <label style={{ fontSize: '12px' }}>Last name</label>
                                <Form.Input
                                    type="text"
                                    name="lastName"
                                    value={props.form.lastName || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <div className="field">
                                <label style={{ fontSize: '12px' }}>Bilkent email adress</label>
                                <Form.Input
                                    type="email"
                                    name="email"
                                    value={props.form.email || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <div className="field">
                                <label style={{ fontSize: '12px' }}>Password</label>
                                <Form.Input
                                    type="password"
                                    name="password"
                                    value={props.form.password || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <div className="field">
                                <label style={{ fontSize: '12px' }}>Re-enter password</label>
                                <Form.Input
                                    type="password"
                                    name="passwordRe"
                                    value={props.form.passwordRe || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <Button onClick={props.onSubmit} fluid positive className="Sign in button" type="submit">
                                Create your BilHub account
                            </Button>
                        </Form>
                    </Segment>
                    <div className="ui center aligned segment">
                        <p
                            style={{
                                display: 'inline',
                                fontSize: '12px',
                            }}>
                            Already have an account? &nbsp;
                        </p>
                        <Link
                            style={{
                                fontSize: '12px',
                            }}
                            to="/login">
                            Login here.
                        </Link>
                    </div>
                </Grid.Column>
            </Grid>
        </div>
    );
};
