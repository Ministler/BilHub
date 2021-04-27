import React, { Component } from 'react';
import { Grid, Segment, Tab, Icon, Accordion } from 'semantic-ui-react';
import './Course.css';

export const GroupsTab = (props) => {
    const divGroupFormed = dummyGroupsFormed.map((group, index) => {
        const contentGroup = group.map((content) => {
            return <p style={{ textAlign: 'center' }}>{content}</p>;
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

    const divGroupUnformed = dummyGroupsUnformed.map((group, index) => {
        const contentGroup = group.map((content) => {
            return <p style={{ textAlign: 'center' }}>{content}</p>;
        });
        return (
            <div class="four wide column">
                <Segment secondary style={{ marginBottom: '25px' }}>
                    {contentGroup}
                </Segment>
            </div>
        );
    });

    if (divGroupUnformed.length !== 0)
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

const dummyGroupsFormed = [
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
];

const dummyGroupsUnformed = [
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Yusuf Uyar', 'Barış Ogün Yörük', 'Oğuzhan Özçelik'],
    ['Yusuf Uyar', 'Barış Ogün Yörük', 'Oğuzhan Özçelik'],
    ['Keke. One', 'Keke. Three', 'Keke. Five'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Mr. One'],
    ['Keke. Five'],
    ['Mr. One'],
    ['Mr. One'],
    ['Mr. One'],
    ['Dummy. Two'],
    ['Dummy. Two'],
    ['Keke. Five'],
];
