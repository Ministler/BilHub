import React from 'react';
import { Form, Button, Grid, Segment } from 'semantic-ui-react';
import { Link } from 'react-router-dom';

import './LoginUI.css';

export const LoginUI = (props) => {
    return (
        <div>
            <Grid centered>
                <Grid.Column style={{ maxWidth: 300, marginTop: 50 }}>
                    <h2 className="ui center aligned icon header">
                        <i className="circular users icon"></i>
                        Sign in to BilHub
                    </h2>
                    {props.error && (
                        <div className="ui negative message" style={{ fontSize: '12px' }}>
                            <i
                                className="close icon"
                                onClick={() => {
                                    props.onErrorClosed();
                                }}></i>
                            {props.error}
                        </div>
                    )}
                    <Segment>
                        <Form className="Sign in form">
                            <div className="field">
                                <label style={{ fontSize: '12px' }}>Bilkent email adress</label>
                                <Form.Input
                                    type="email"
                                    name="email"
                                    value={props.form?.email || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <div className="field">
                                <Link
                                    style={{
                                        fontSize: '12px',
                                        float: 'right',
                                    }}
                                    to="/newPassword">
                                    Forgot password?
                                </Link>
                                <label style={{ fontSize: '12px' }}>Password</label>
                                <Form.Input
                                    type="password"
                                    name="password"
                                    value={props.form.password || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <Button onClick={props.onSubmit} fluid positive className="Sign in button" type="submit">
                                Login
                            </Button>
                        </Form>
                    </Segment>
                    <div className="ui center aligned segment">
                        <p
                            style={{
                                display: 'inline',
                                fontSize: '12px',
                            }}>
                            New to BilHub? &nbsp;
                        </p>
                        <Link
                            style={{
                                fontSize: '12px',
                            }}
                            to="/signup">
                            Create an account.
                        </Link>
                    </div>
                </Grid.Column>
            </Grid>
        </div>
    );
};
