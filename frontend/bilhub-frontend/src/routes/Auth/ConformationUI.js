import React from 'react';
import { Form, Button, Grid, Segment, Image } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import logo from '../../assets/logo.png';

export const ConformationUI = (props) => {
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
                        <div className="ui positive message" style={{ fontSize: '12px' }}>
                            <i className="close icon" onClick={props.onPopupClosed}></i>
                            {props.information}
                        </div>
                    )}
                    <Segment>
                        <Form className="Conformation form" onSubmit={props.onConformation}>
                            <div className="field">
                                <label style={{ fontSize: '12px', display: 'inline' }}>Conformation Code</label>
                                <Link
                                    onClick={props.onResendCode}
                                    style={{
                                        fontSize: '12px',
                                        float: 'right',
                                    }}>
                                    Resend the Code
                                </Link>
                                <Form.Input
                                    type="text"
                                    name="conformationCode"
                                    value={props.form.conformationCode || ''}
                                    onChange={props.onChange}
                                    acti
                                />
                            </div>
                            <Button fluid positive className="Sign in button" type="submit">
                                Confirm Your Account
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
