import React, { Component } from 'react';
import {
    Divider,
    Input,
    Dropdown,
    Grid,
    Checkbox,
    Segment,
    GridColumn,
    Button,
    GridRow,
    Icon,
    Form,
    Message,
    List,
    Popup,
} from 'semantic-ui-react';

import './CourseSettings.css';

import { GroupsTab } from '../GroupsTab';

import { FormedGroupModal, FormationGroupModal } from '../CourseComponents';

import { convertFormedGroupsToBriefList, convertUnformedGroupsToBriefList } from '../../../components';

const semesterOptions = [
    {
        key: 'Fall',
        text: 'Fall',
        value: 'Fall',
    },
    {
        key: 'Spring',
        text: 'Spring',
        value: 'Spring',
    },
    {
        key: 'Summer',
        text: 'Summer',
        value: 'Summer',
    },
];

const groupFormationSettings = [
    {
        key: 'By Group Size',
        text: 'By Group Size',
        value: 'By Group Size',
    },
    {
        key: 'By Group Number',
        text: 'By Group Number',
        value: 'By Group Number',
    },
];
export class CourseSettings extends Component {
    constructor(props) {
        super(props);
        this.settingTitle = 'Course Settings ' + dummyCourseInformation.courseName;
        var code;
        var year;
        var semester;
        var dash = 0;
        for (var i = 0; i < dummyCourseInformation.courseName.length; i++) {
            if (dummyCourseInformation.courseName.charAt(i) == '-') {
                code = dummyCourseInformation.courseName.substring(0, i);
                dash = i;
            } else if (dummyCourseInformation.courseName.charAt(i) == ' ') {
                year = parseInt(dummyCourseInformation.courseName.substring(dash + 1, i));
                dash = i;
            } else if (i + 1 == dummyCourseInformation.courseName.length) {
                semester = dummyCourseInformation.courseName.substring(dash + 1);
            }
        }
        var sectionNumber = dummyCourseInformation.sectionNumber;
        var sections = [];
        for (var i = 1; i <= sectionNumber; i++) {
            sections.push({
                key: i,
                text: i,
                value: i,
            });
        }
        console.log(semester);
        this.state = {
            code: code,
            year: year,
            semester: semester,
            isSectionless: false,
            sectionNumber: sectionNumber,
            sections: sections,
            instructorList: [],
            TAList: [],
            studentManualList: [],
            studentAutoList: [],
            groupFormationType: dummyCourseInformation.groupFormationType,
            isModalOpen: false,
            curGroupModal: {},
        };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event, data) {
        const name = data.name;
        var value = '';
        if (data.type === 'checkbox') {
            value = data.checked;
            var sectionNumber = value ? 0 : 1;
            this.setState({ [name]: value, sectionNumber: sectionNumber });
        } else {
            value = data.value;
            this.setState({
                [name]: value,
            });
        }
    }

    changeSection = (event, data) => {
        var sections = [];
        for (var i = 1; i <= data.value; i++) {
            sections.push({
                key: i,
                text: i,
                value: i,
            });
        }
        console.log(sections);
        this.setState({ sections: sections });
        this.handleChange(event, data);
    };

    createUserList(members) {
        return (
            <Segment style={{ height: '200px' }}>
                <List items={members}></List>
                <Segment.Inline className="AddSegment">
                    <Input icon={<Icon name="plus" inverted circular link />} placeholder="Enter" />
                </Segment.Inline>
            </Segment>
        );
    }

    render() {
        return (
            <>
                <Form>
                    <Form.Group>
                        <h1>{this.settingTitle}</h1>
                    </Form.Group>
                    <Divider />
                    <Form.Group>
                        <Form.Field width={3}>
                            <label for="code">Code:</label>{' '}
                            <Form.Input
                                value={this.state.code}
                                onChange={this.handleChange}
                                name="code"
                                style={{ width: '50%' }}
                            />
                        </Form.Field>
                        <Form.Field width={3}>
                            <label for="year">Year:</label>
                            <Form.Input
                                value={this.state.year}
                                min="0"
                                name="year"
                                style={{ width: '50%' }}
                                onChange={this.handleChange}
                                type="number"
                            />
                        </Form.Field>
                        <Form.Field width={3}>
                            <label for="semester">Semester:</label>
                            <Form.Dropdown
                                value={this.state.semester}
                                onChange={this.handleChange}
                                name="semester"
                                inline
                                placeholder="Select"
                                selection
                                options={semesterOptions}
                            />
                        </Form.Field>
                        <Form.Field width={7} textAlign="center">
                            {this.state.code}
                            {(this.state.code != '' || this.state.code != '') && '-'}
                            {this.state.year} {this.state.semester}
                        </Form.Field>
                    </Form.Group>
                    <Divider />
                    <Grid>
                        <Grid.Row columns={6}>
                            <GridColumn>Add Instructor:</GridColumn>
                            <GridColumn>{this.createUserList(this.state.instructorList)}</GridColumn>
                            <GridColumn>Add Teaching Assistants</GridColumn>
                            <GridColumn>{this.createUserList(this.state.instructorList)}</GridColumn>
                        </Grid.Row>
                        <Grid.Row columns={6}>
                            <GridColumn>
                                <div>Add Student as .txt file:</div>
                                {this.state.sectionNumber > 0 && (
                                    <div>
                                        Section:
                                        <Dropdown
                                            fluid
                                            selection
                                            options={this.state.sections}
                                            defaultValue={this.state.sections[0].value}
                                        />
                                    </div>
                                )}
                            </GridColumn>
                            <GridColumn>
                                <Button>Add File</Button>
                            </GridColumn>
                            <GridColumn>
                                <div>Add Student as a list:</div>
                                {this.state.sectionNumber > 0 && (
                                    <div>
                                        Section:{' '}
                                        <Dropdown
                                            fluid
                                            selection
                                            options={this.state.sections}
                                            defaultValue={this.state.sections[0].value}
                                        />
                                    </div>
                                )}
                            </GridColumn>
                            <GridColumn>{this.createUserList(this.state.instructorList)}</GridColumn>
                        </Grid.Row>
                    </Grid>
                    <Divider />
                    <Form.Group>
                        <Form.Field>
                            Group Formation Type:
                            <Dropdown
                                style={{ float: 'left' }}
                                name="groupFormationType"
                                onChange={this.handleChange}
                                fluid
                                selection
                                options={groupFormationSettings}
                                value={this.state.groupFormationType}></Dropdown>
                            {this.state.groupFormationType == '' && (
                                <Popup
                                    style={{ float: 'left' }}
                                    content={'Select one of the group formations'}
                                    header={'Group Formation'}
                                    trigger={<Icon name="info circle"></Icon>}
                                />
                            )}
                            {this.state.groupFormationType == 'By Group Size' && (
                                <span>
                                    <Popup
                                        style={{ float: 'left' }}
                                        content={'Select one of the group formations'}
                                        header={'Group Formation'}
                                        trigger={<Icon name="info circle"></Icon>}
                                    />
                                    <div>
                                        Min:<Input type="number"></Input> Max:<Input type="number"></Input>
                                        Group Formation Date
                                        <Input type="date"></Input>
                                    </div>
                                </span>
                            )}
                            {this.state.groupFormationType == 'By Group Number' && (
                                <span>
                                    <Popup
                                        style={{ float: 'left' }}
                                        content={'Select one of the group formations'}
                                        header={'Group Formation'}
                                        trigger={<Icon name="info circle"></Icon>}
                                    />
                                    <div>
                                        Group Number:<Input type="number"></Input>
                                        Group Formation Date
                                        <Input type="date"></Input>
                                    </div>
                                </span>
                            )}
                        </Form.Field>
                    </Form.Group>
                    <Divider />
                    <Button>Change Group Settings</Button>
                    <Divider />
                    <h1>Change Groups</h1>
                    Section:
                    <Form.Field>
                        <Dropdown
                            fluid
                            selection
                            options={this.state.sections}
                            defaultValue={this.state.sections[0].value}
                        />
                    </Form.Field>
                    <Divider />
                    {dummyCourseInformation.courseState === 'formation' &&
                        convertUnformedGroupsToBriefList({
                            groups: dummyGroupsInFormation,
                        })}
                    {dummyCourseInformation.courseState === 'formed' &&
                        convertFormedGroupsToBriefList({
                            groups: dummyGroupsInFormation,
                        })}
                </Form>
            </>
        );
    }
}

const dummyCourseInformation = {
    courseName: 'CS319-2021 Spring',
    description: 'Object-Oriented Software Engineering',
    isCourseActive: true,
    instructors: [
        {
            name: 'Eray Tüzün',
            information: 'eraytuzun@gmail.com',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            information: 'alpersarikan@gmail.com',
            userId: 2,
        },
    ],
    TAs: [
        {
            name: 'Erdem Tuna',
            information: 'erdemtuan@gmail.com',
            userId: 1,
        },
        {
            name: 'Kraliçe Irmak',
            information: 'kraliceirmak@gmail.com',
            userId: 2,
        },
    ],
    information:
        'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Odio, et assumenda fugiat repudiandae doloribus eaque at possimus tenetur cum ratione, non voluptatibus? Provident nam cum et cupiditate corporis earum vel ut? Illum beatae molestiae praesentium cumque sapiente, quasi neque consequatur distinctio iste possimus in dolor. Expedita rem totam ex distinctio!',
    isUserInstructorOfCourse: true,
    isUserTAOfCourse: true,
    sectionNumber: 3,
    groupFormationType: 'By Group Number',
    courseState: 'formed',
};

const dummyGroupsInFormation = [
    {
        name: 'Bilhub',
        members: ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    },
    { name: 'Not Bilhub', members: ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'] },
    {
        name: 'Bilhub2',
        members: ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    },
    { name: 'Keke Yusuf', members: ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'] },
];

const dummyGroupsFormed = [
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. Pepe'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
];
