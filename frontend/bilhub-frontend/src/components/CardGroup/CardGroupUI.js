import React from 'react';
import { Card, Grid, Icon } from 'semantic-ui-react';
import { dateObjectToString, convertDate } from '../../utils';

import './CardGroupUI.css';

export const AssignmentCardElement = (props) => {
    return (
        <Card className="AssignmentCard">
            <Card.Content>
                <Card.Header>
                    <span className="clickableChangeColor" onClick={props.titleClicked}>
                        {props.title}
                    </span>
                    <span>{props.titleIcon}</span>
                </Card.Header>
                <Card.Description>
                    <div>{props.caption}</div>
                    <span onClick={props.fileClicked} className="clickableChangeColor docFile">
                        {props.fileIcon}
                    </span>
                </Card.Description>
            </Card.Content>
            <Card.Content extra textAlign="right">
                <div>{props.publisher}</div>
                <div>{typeof props.date === 'object' ? convertDate(dateObjectToString(props.date)) : convertDate(props.date)}</div>
            </Card.Content>
        </Card>
    );
};

export const FeedbackCardElement = (props) => {
    return (
        <Card className="FeedbackCardElement" >
            {props?.isSrs !== true && (
                <Card.Content>
                    <Card.Header>
                        {props.author ? (
                            <div onClick={props.onAuthorClicked}>{props.author}</div>
                        ) : (
                            <div>{props.titleElement}</div>
                        )}
                    </Card.Header>
                    <Card.Description>
                        {props.caption}
                        {props.hasFile ? <Icon name="file" onClick={props.onFeedbackFileClicked} /> : null}
                        {props.icons}
                    </Card.Description>
                </Card.Content>
            )}
            <Card.Content className="FeedbackCardExtra">
                <div className="FeedbackGrade">
                    Grade: {props.grade}/{props.maxGrade ? props.maxGrade : '10'}
                </div>
                <div className="FeedbackDate">
                    {typeof props.date === 'object' ? convertDate(dateObjectToString(props.date) ): convertDate(props.date)}
                </div>
            </Card.Content>
        </Card>
    );
};

export const RequestCardElement = (props) => {
    // {props.titleStart} {props.userName} {props.titleMid} {props.courseName}
    // {props.message}
    return (
        <Card className="FeedbackCardElement">
            <Card.Content>
                <Card.Header>
                    <div>{props.courseName}</div>
                </Card.Header>
                <p style={{ fontSize: '15px', fontWeight: 'bold' }}>
                    {props.titleStart} {props.userName} {props.titleMid}
                </p>
                <Card.Description>
                    <Grid columns={2}>
                        <Grid.Column>
                            {props.yourGroup ? (
                                <>
                                    <label style={{ fontSize: '14px', fontWeight: 'bold' }}>Your Group</label>
                                    {props.yourGroup}
                                </>
                            ) : null}
                        </Grid.Column>
                        <Grid.Column>
                            {props.otherGroup ? (
                                <>
                                    <label style={{ fontSize: '14px', fontWeight: 'bold' }}>Their Group</label>
                                    {props.otherGroup}
                                </>
                            ) : null}
                        </Grid.Column>
                    </Grid>
                    <p style={{ marginTop: '15px' }}>{props.message}</p>
                </Card.Description>
            </Card.Content>
            <Card.Content>
                <div style={{ float: 'left' }}>
                    <span style={{ marginLeft: '10px' }}>{props.voteIcons}</span>
                </div>
                <div style={{ float: 'right' }}>
                    {props.requestDate ? (
                        <span>
                            Request Date:{' '}
                            {typeof props.requestDate === 'object'
                                ? convertDate(dateObjectToString(props.requestDate))
                                : convertDate(props.requestDate)}{' '}
                            / Formation Date:{' '}
                            {typeof props.formationDate === 'object'
                                ? convertDate(dateObjectToString(props.formationDate))
                                : convertDate(props.formationDate)}
                        </span>
                    ) : null}
                    {props.approvalDate ? (
                        <span>
                            Approval Date:{' '}
                            {typeof props.approvalDate === 'object'
                                ? convertDate(dateObjectToString(props.approvalDate))
                                : convertDate(props.approvalDate)}
                        </span>
                    ) : null}
                    {props.disapprovalDate ? (
                        <span>
                            Dispproval Date:{' '}
                            {typeof props.disapprovalDate === 'object'
                                ? convertDate(dateObjectToString(props.disapprovalDate))
                                : convertDate(props.disapprovalDate)}
                        </span>
                    ) : null}
                </div>
            </Card.Content>
        </Card>
    );
};
