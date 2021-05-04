import React from 'react';
import { Grid, Segment, Label } from 'semantic-ui-react';
import './CourseComponents.css';
import { Accordion } from '../../../components';
import { Link } from 'react-router-dom';

export const GroupsTab = (props) => {
    const divGroupFormed = props.groupsFormed?.map((group, index) => {
        const contentGroup = group?.members?.map((member) => {
            return (
                <p style={{ textAlign: 'center' }}>
                    <Link className="courseGroupMember">{member.name}</Link>
                </p>
            );
        });

        let onGroupClicked = null;
        if (props.isLocked) {
            onGroupClicked = () => props.onGroupClicked(group.groupId);
        }

        if (index % 9 === 0)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(255, 0, 0, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(255, 0, 0, 0.3)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 1)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(162 , 167 , 0, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(162 , 167 , 0, 0.4)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 2)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(0, 0, 255, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(0, 0, 255, 0.3)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 3)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(255, 255, 0, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(255, 215, 59, 0.6)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 4)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(255, 0, 255, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(255, 0, 255, 0.3)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 5)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(0, 255, 255, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(0, 255, 255, 1)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 6)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(255, 150, 30, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(255, 150, 30, 0.5)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 7)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(0, 255, 0, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(0, 255, 0, 0.5)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 8)
            return (
                <div class="four wide column">
                    <Segment
                        onClick={onGroupClicked}
                        style={{ backgroundColor: 'rgba(3 , 94 , 123, 0.2)', marginBottom: '25px' }}>
                        {group.groupName && (
                            <Label
                                style={{
                                    textAlign: 'center',
                                    backgroundColor: 'rgba(3 , 94 , 123, 0.3)',
                                    color: 'rgb(0,0,0)',
                                }}
                                attached="top">
                                {group.groupName}
                            </Label>
                        )}
                        {contentGroup}
                    </Segment>
                </div>
            );
        return <>There Is Not Any Groups</>;
    });

    const divGroupUnformed = props.groupsUnformed?.map((group, index) => {
        const contentGroup = group?.members?.map((member) => {
            return <p style={{ textAlign: 'center' }}>{member.name}</p>;
        });
        let groupClickedHandler = null;
        if (!props.isUserInFormedGroup && props.isUserInThisSection) {
            if (group.isUserInGroup) {
                groupClickedHandler = () =>
                    props.onUnformedGroupModalOpened(
                        group.groupId,
                        group.voteStatus,
                        group.members,
                        group.isUserReady,
                        group.isFormable
                    );
            } else if (!group.notRequestable) {
                groupClickedHandler = () => props.onSendRequestModalOpened(group.groupId, group.members);
            }
        }
        return (
            <div class="four wide column">
                <Segment onClick={groupClickedHandler} secondary style={{ marginBottom: '25px' }}>
                    {contentGroup}
                </Segment>
            </div>
        );
    });

    if (divGroupUnformed) {
        let accordionPanes = [];
        if (divGroupFormed) {
            accordionPanes.push({
                title: 'Formed Groups',
                content: (
                    <div>
                        <Grid columns="equal">
                            <Grid.Row>{divGroupFormed}</Grid.Row>
                        </Grid>
                    </div>
                ),
            });
        }
        accordionPanes.push({
            title: 'Unformed Groups',
            content: (
                <div>
                    <Grid columns="equal">
                        <Grid.Row>{divGroupUnformed}</Grid.Row>
                    </Grid>
                </div>
            ),
        });

        return <Accordion accordionSections={accordionPanes} />;
    } else {
        if (divGroupFormed) {
            return (
                <Grid columns="equal" style={{ 'margin-top': '10px' }}>
                    <Grid.Row>{divGroupFormed}</Grid.Row>
                </Grid>
            );
        }
    }
    return;
};
