import React, { Component } from 'react';
import { Grid, Segment, Tab, Icon } from 'semantic-ui-react';
import './Course.css';
import { GroupsTab } from './GroupsTab';

export class Course extends Component {
    panes = [
        {
            menuItem: 'Groups',
            render: () => (
                <Tab.Pane as="div" attached={false}>
                    <GroupsTab groupsFormed={dummyGroupsFormed} groupsUnformed={dummyGroupsUnformed}></GroupsTab>
                </Tab.Pane>
            ),
        },
        {
            menuItem: 'Statistics',
            render: () => (
                <Tab.Pane as="div" attached={false}>
                    Statistics
                </Tab.Pane>
            ),
        },
        {
            menuItem: 'Assignment',
            render: () => (
                <Tab.Pane as="div" attached={false}>
                    Assignment
                </Tab.Pane>
            ),
        },
    ];

    TabExampleSecondaryPointing = () => (
        <Tab menu={{ secondary: true, pointing: true, color: 'red' }} style={{ width: '75%' }} panes={this.panes} />
    );
    render() {
        return (
            <div class="ui centered grid">
                <div class="row">
                    <div class="four wide column">
                        <Segment>
                            <h1 className="ui center aligned header">Course Tag</h1>
                            <h3 className="ui center aligned header">Course Name</h3>
                            <h4 style={{ marginLeft: '20px' }}>Instructor</h4>
                            <p>Instructors</p>
                            <h4 style={{ marginLeft: '20px' }}>TA's</h4>
                            <p>TA's</p>
                            <h4 style={{ marginLeft: '20px' }}>Information</h4>
                            <p>
                                Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eaque fugit ipsum totam soluta
                                aperiam dicta laudantium eligendi nobis repellendus ex commodi culpa corporis, aut
                                ducimus eius temporibus debitis, harum eveniet!
                            </p>
                        </Segment>
                    </div>
                    <div class="twelve wide column">
                        <this.TabExampleSecondaryPointing></this.TabExampleSecondaryPointing>
                    </div>
                </div>
            </div>
        );
    }
}

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
    ['Yusuf Uyar', 'Barış Ogün Yörük', 'Oğuzhan Özçelik'],
    ['Yusuf Uyar', 'Barış Ogün Yörük', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Ahmet Demir', 'Muhammed Arshellov'],
    ['Mr. One'],
    ['Keke. Five'],
    ['Mr. One'],
    ['Mr. One'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Three', 'Keke. Five'],
    ['Mr. One'],
    ['Dummy. Two'],
    ['Dummy. Two'],
    ['Keke. Five'],
];
