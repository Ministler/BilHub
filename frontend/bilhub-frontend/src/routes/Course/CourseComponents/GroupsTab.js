import React, { Component } from 'react';
import { Grid, Segment, Tab, Icon, Accordion } from 'semantic-ui-react';

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

    const divGroupUnformed = props.groupsUnformed?.map((group) => {
        const contentGroup = group?.members?.map((member) => {
            return <p style={{ textAlign: 'center' }}>{member.name}</p>;
        });
        return (
            <div class="four wide column">
                <Segment secondary style={{ marginBottom: '25px' }}>
                    {contentGroup}
                </Segment>
            </div>
        );
    });

    if (props.groupsUnformed?.length !== 0)
        return (
            <div class="accordion ui fluid">
                <div class="active title">
                    <i aria-hidden="true" class="dropdown icon"></i>Formed Groups
                </div>
                <div class="content active">
                    <Grid columns="equal">
                        <Grid.Row>{divGroupFormed}</Grid.Row>
                    </Grid>
                </div>
                <div class="active title">
                    <i aria-hidden="true" class="dropdown icon"></i>Unformed Groups
                </div>
                <div class="content active">
                    <Grid columns="equal">
                        <Grid.Row>{divGroupUnformed}</Grid.Row>
                    </Grid>
                </div>
            </div>
        );
    else
        return (
            <Grid columns="equal">
                <Grid.Row>{divGroupFormed}</Grid.Row>
            </Grid>
        );
};
