import React from 'react';
import { Form, Button, Grid, Segment, Image } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import logo from '../../assets/logo.png';

export const NewPasswordUI = (props) => {
    return (
        <div>
            <Grid centered>
                <Grid.Column style={{ maxWidth: 300, marginTop: 50 }}>
                    <Image src={logo} size="small" centered></Image>
                    <h2 className="ui center aligned icon header">Reset your password</h2>
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
                                <label style={{ fontSize: '12px' }}>Please type your Bilkent email</label>
                                <Form.Input
                                    type="email"
                                    name="email"
                                    value={props.form?.email || ''}
                                    onChange={props.onChange}
                                />
                            </div>
                            <Button onClick={props.onSubmit} fluid positive className="Sign in button" type="submit">
                                Send new password
                            </Button>
                        </Form>
                    </Segment>
                    <div className="ui center aligned segment">
                        <Link
                            style={{
                                fontSize: '12px',
                            }}
                            to="/login">
                            Go back to Login Page
                        </Link>
                    </div>
                </Grid.Column>
            </Grid>
        </div>
    );
};
