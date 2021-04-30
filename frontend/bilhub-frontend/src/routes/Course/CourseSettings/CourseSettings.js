import React, { Component } from 'react';
import _ from 'lodash';
import {
    Divider,
    Input,
    Dropdown,
    Grid,
    Segment,
    GridColumn,
    Button,
    Icon,
    Form,
    List,
    Popup,
    TextArea,
} from 'semantic-ui-react';
import { UserSearchBar } from '../CourseComponents';

import './CourseSettings.css';

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
        var studentManualList = [];
        var studentAutoList = [];
        for (var i = 0; i < sectionNumber; i++) {
            studentAutoList.push([]);
            studentManualList.push([]);
        }
        var sections = [];
        for (var i = 1; i <= sectionNumber; i++) {
            sections.push({
                key: i,
                text: i,
                value: i,
            });
        }
        var date =
            dummyCourseInformation.groupFormationDate.getFullYear() +
            '-' +
            dummyCourseInformation.groupFormationDate.getMonth() +
            '-' +
            dummyCourseInformation.groupFormationDate.getDate();

        this.state = {
            code: code,
            year: year,
            semester: semester,
            shortDescription: dummyCourseInformation.description,
            isSectionless: dummyCourseInformation.isSectionless,
            sectionNumber: sectionNumber,
            sections: sections,
            instructorList: [],
            currentInstructor: '',
            TAList: [],
            currentTA: '',
            studentManualList: studentManualList,
            manualSection: 1,
            currentStudent: '',
            studentAutoList: studentAutoList,
            autoSection: 1,
            minSize: dummyCourseInformation.minSize,
            maxSize: dummyCourseInformation.maxSize,
            groupFormationDate: date,
        };
        console.log(this.state.groupFormationDate);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event, data) {
        event.preventDefault();

        if (!this.validations(data)) {
            return;
        }
        const name = data.name;
        var value = '';
        value = data.value;
        this.setState({
            [name]: value,
        });
    }

    validations = (data) => {
        const name = data.name;
        switch (name) {
            case 'code':
                if (String(data.value).length > 7) return false;
                break;
        }

        return true;
    };

    removeUser = (element, listType, section) => {
        var ary = this.state[listType];
        if (section === 0) {
            ary = _.without(ary, element);
        } else {
            ary[section - 1] = _.without(ary[section - 1], element);
        }
        this.setState({ [listType]: ary });
    };

    createUserList(members, userType, listType, section = 0) {
        var list = section === 0 ? members : members[section - 1];
        return (
            <Segment style={{ height: '200px' }}>
                <List selection className="UserList" items={list}>
                    {list.map((element) => {
                        return (
                            <Popup
                                on="click"
                                content={
                                    <Button onClick={() => this.removeUser(element, listType, section)} color="red">
                                        Remove User
                                    </Button>
                                }
                                trigger={<List.Item>{element}</List.Item>}
                            />
                        );
                    })}
                </List>
                <Segment.Inline className="AddSegment">
                    <Form.Input
                        name={userType}
                        onChange={this.handleChange}
                        icon={
                            <Icon
                                name="plus"
                                inverted
                                circular
                                link
                                onClick={() => {
                                    this.addUser(userType, listType, section);
                                }}
                            />
                        }
                        onKeyDown={(e) => {
                            if (e.key === 'Enter') {
                                this.addUser(userType, listType, section);
                            }
                        }}
                        placeholder="Enter"
                        value={this.state[userType]}
                    />
                </Segment.Inline>
            </Segment>
        );
    }

    addUser = (userType, listType, section) => {
        if (this.state[userType] === '') {
            return;
        }
        for (var i = 0; i < this.state[userType].length; i++)
            if (
                this.state[userType][i] === '@' &&
                i + 1 < this.state[userType].length &&
                this.state[userType].indexOf('bilkent', i + 1) === -1
            ) {
                window.alert('Please enter bilkent email');
                return;
            }
        let curList = this.state[listType];
        if (section === 0) {
            if (this.checkIfExists(curList, this.state[userType])) {
                window.alert("You can't add already existing user.");
                return;
            }
            curList.push(this.state[userType]);
        } else {
            for (var i = 0; i < curList.length; i++) {
                if (this.checkIfExists(curList[i], this.state[userType])) {
                    window.alert("You can't add already existing user.");
                    return;
                }
            }
            curList[section - 1].push(this.state[userType]);
        }
        this.setState({ [listType]: curList, [userType]: '' });
    };

    checkIfExists = (list, element) => {
        for (var i = 0; i < list.length; i++) {
            if (element === list[i]) {
                return true;
            }
        }
        return false;
    };

    onFormSubmit = (e) => {
        console.log(this.state.groupFormationDate);
        for (var i = 0; i < this.state.sectionNumber; i++) {
            for (var k = 0; k < this.state.studentManualList[i].length; k++) {
                this.state.studentAutoList[i].push(this.state.studentManualList[i][k]);
            }
        }
        var dateArr = this.state.groupFormationDate.split(/-/);
        //System.DateTime
        var d = new Date(dateArr[0], dateArr[1], dateArr[2]);
        return {
            courseName: this.state.code + '/' + this.state.year + this.state.semester,
            description: this.state.shortDescription,
            isSectionless: this.state.isSectionless,
            numberOfSection: this.state.sectionNumber, // If sectionless this field is 1

            groupFormationType: this.state.groupFormationType,
            newStudents: this.state.studentAutoList,
            newTAs: this.state.TAList,
            newinstructors: this.state.instructorList,

            minSize: this.state.minSize,
            maxSize: this.state.maxSize,
            groupFormationDate: d,
        };
    };

    readFile = (e) => {
        const reader = new FileReader();
        var students;
        reader.onload = async (file) => {
            var curList = this.state.studentAutoList;
            const text = file.target.result;
            students = text.split(/\n/);
            curList[this.state.autoSection - 1] = students;
            this.setState({ studentAutoList: curList /* currentFile: */ });
        };
        reader.readAsText(e.target.files[0]);
    };

    removeUserFromCourse = (userMail) => {
        console.log(userMail + ' will be removed');
    };

    render() {
        return (
            <>
                <Form className="SettingsForm" onSubmit={this.onFormSubmit}>
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
                        <Form.Field className="newCourseName" width={7} textAlign="center">
                            <h2>
                                {this.state.code}
                                {(this.state.code != '' || this.state.code != '') && '-'}
                                {this.state.year} {this.state.semester}
                            </h2>
                        </Form.Field>
                    </Form.Group>
                    <Form.Group>
                        <label for="shortDescription">Short Course Description:</label>{' '}
                        <TextArea
                            className="Description"
                            value={this.state.shortDescription}
                            onChange={this.handleChange}
                            name="shortDescription"
                            style={{ width: '50%', height: '42px' }}
                        />
                    </Form.Group>
                    <Divider />
                    <Grid>
                        <Grid.Row>
                            <GridColumn width={3}>Add Instructor:</GridColumn>
                            <GridColumn width={5}>
                                {this.createUserList(this.state.instructorList, 'currentInstructor', 'instructorList')}
                            </GridColumn>
                            <GridColumn width={3}>Add Teaching Assistants</GridColumn>
                            <GridColumn width={5}>
                                {this.createUserList(this.state.TAList, 'currentTA', 'TAList')}
                            </GridColumn>
                        </Grid.Row>
                        <Grid.Row>
                            <GridColumn width={3}>
                                <div>Add Student as .txt file:</div>
                                {this.state.isSectionless !== true && (
                                    <div>
                                        Section:
                                        <Dropdown
                                            name="autoSection"
                                            fluid
                                            selection
                                            options={this.state.sections}
                                            defaultValue={this.state.sections[0].value}
                                            onChange={this.handleChange}
                                        />
                                    </div>
                                )}
                            </GridColumn>
                            <GridColumn width={5}>
                                <input
                                    className="FileInput"
                                    type="file"
                                    accept=".txt"
                                    value={this.state.currentFile}
                                    onChange={(e) => this.readFile(e)}
                                />
                            </GridColumn>
                            <GridColumn width={3}>
                                <div>Add Student as a list:</div>
                                {this.state.isSectionless !== true && (
                                    <div>
                                        Section:{' '}
                                        <Dropdown
                                            name="manualSection"
                                            fluid
                                            selection
                                            options={this.state.sections}
                                            defaultValue={this.state.sections[0].value}
                                            onChange={this.handleChange}
                                        />
                                    </div>
                                )}
                            </GridColumn>
                            <GridColumn width={5}>
                                {this.createUserList(
                                    this.state.studentManualList,
                                    'currentStudent',
                                    'studentManualList',
                                    this.state.manualSection
                                )}
                            </GridColumn>
                        </Grid.Row>
                    </Grid>
                    <Divider />
                    <Form.Group>
                        <Form.Field>
                            <h2>Group Formation Settings</h2>
                            <div>
                                Min Group Size:
                                <Input
                                    name="minSize"
                                    value={this.state.minSize}
                                    min={1}
                                    max={this.state.maxSize}
                                    onChange={this.handleChange}
                                    type="number"></Input>{' '}
                                Max Group Size:
                                <Input
                                    name="maxSize"
                                    min={this.state.minSize}
                                    value={this.state.maxSize}
                                    onChange={this.handleChange}
                                    type="number"></Input>
                                Group Formation Date
                                <Input
                                    value={this.state.groupFormationDate}
                                    type="date"
                                    name="groupFormationDate"
                                    onChange={this.handleChange}></Input>
                            </div>
                        </Form.Field>
                    </Form.Group>
                    <Divider />
                    <Button>Change Group Settings</Button>
                    <Button
                        color="red"
                        content="Delete Course"
                        icon="exclamation"
                        style={{ marginLeft: '10px' }}
                        onClick={() => {
                            console.log('delete course');
                        }}
                    />
                    <Divider />
                    <h2>Remove Students From Course</h2>
                    <UserSearchBar users={dummyUsers} removeUserFromCourse={this.removeUserFromCourse} />
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
    isSectionless: false,
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
    courseState: 'formed',
    minSize: 3,
    maxSize: 5,
    groupFormationDate: new Date(2022, 11, 14),
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
    {
        name: 'Keke Yusuf',
        members: ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. onErrorCleaned'],
    },
];

const dummyGroupsFormed = [
    ['Yusuf Uyar', 'Halil Özgür Demir', 'Barış Ogün Yörük', 'Aybala Karakaya', 'Oğuzhan Özçelik'],
    ['Ahmet Demir', 'Altay Bastık', 'Cemre Güçlü', 'Muhammed Arshellov', 'Mr. onErrorCleaned'],
    ['Mr. One', 'Mr. Two', 'Miss. Three', 'Mr. Four', 'Miss. Five'],
    ['Dummy. One', 'Dummy. Two', 'Dummy. Three', 'Dummy. Four', 'Dummy. Five'],
    ['Keke. One', 'Keke. Two', 'Keke. Three', 'Keke. Four', 'Keke. Five'],
];

const dummyUsers = [
    { type: 'student', mail: 'yusuf@bilkent.edu.tr' },
    { type: 'student', mail: 'baris@bilkent.edu.tr' },
    { type: 'student', mail: 'aybala@bilkent.edu.tr' },
    { type: 'student', mail: 'ozgur@bilkent.edu.tr' },
    { type: 'student', mail: 'ozco@bilkent.edu.tr' },
    { type: 'student', mail: 'mert@bilkent.edu.tr' },
    { type: 'student', mail: 'ata@bilkent.edu.tr' },
    { type: 'student', mail: 'asd@bilkent.edu.tr' },
    { type: 'student', mail: 'fgh@bilkent.edu.tr' },
    { type: 'student', mail: 'jkl@bilkent.edu.tr' },
    { type: 'student', mail: 'qwe@bilkent.edu.tr' },
    { type: 'student', mail: 'rety@bilkent.edu.tr' },
    { type: 'student', mail: 'werrd@bilkent.edu.tr' },
    { type: 'student', mail: 'xcccxs@bilkent.edu.tr' },
    { type: 'student', mail: 'bvbvbvb@bilkent.edu.tr' },
    { type: 'student', mail: 'fdgggggw@bilkent.edu.tr' },
    { type: 'TA', mail: 'TA@bilkent.edu.tr' },
    { type: 'TA', mail: 'TA22@bilkent.edu.tr' },
    { type: 'instructor', mail: 'hoca@bilkent.edu.tr' },
];
