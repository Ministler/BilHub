import React from 'react';
import { Grid, Segment } from 'semantic-ui-react';

import { Accordion } from '../../../components';

export const GroupsTab = (props) => {
    const divGroupFormed = props.groupsFormed?.map((group, index) => {
        const contentGroup = group?.members?.map((member) => {
            return <p style={{ textAlign: 'center' }}>{member.name}</p>;
        });

        if (index % 9 === 0)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(255, 0, 0, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 1)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(162 , 167 , 0, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 2)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(0, 0, 255, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 3)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(255, 255, 0, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 4)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(255, 0, 255, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 5)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(0, 255, 255, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 6)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(255, 150, 30, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 7)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(0, 255, 0, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
        if (index % 9 === 8)
            return (
                <div class="four wide column">
                    <Segment style={{ backgroundColor: 'rgba(3 , 94 , 123, 0.2)', marginBottom: '25px' }}>
                        {contentGroup}
                    </Segment>
                </div>
            );
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
};
