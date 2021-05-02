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
import { getCourseRequest, postDeactivateCourseRequest, putCourseRequest, deleteCourseRequest } from '../../../API';
import { FormedGroupsBriefList, convertUnformedGroupsToBriefList } from '../../../components';
import { dateObjectToInputDate, inputDateToDateObject } from '../../../utils';

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

export class CourseSettings extends Component {
    constructor(props) {
        super(props);

        this.state = {
            code: null,
            year: null,
            semester: null,
            shortDescription: null,
            sections: null,
            instructorList: null,
            currentInstructor: null,
            TAList: null,
            currentTA: null,
            studentManualList: null,
            manualSection: null,
            currentStudent: null,
            studentAutoList: null,
            autoSection: null,
            minSize: null,
            maxSize: null,
            groupFormationDate: null,
            groups: null,
            users: null,
            isLocked: null,
            isSectionless: null,
        };
    }

    updateGeneralSettings = () => {
        const arr = [...this.state.studentAutoList];
        for (let i = 0; i < this.state.numberOfSections; i++) {
            arr[i] = [...this.state.studentAutoList[i]];
            for (let k = 0; k < this.state.studentManualList[i].length; k++) {
                arr[i].push(this.state.studentManualList[i][k]);
            }
        }
        //System.DateTime
        let date = inputDateToDateObject(this.state.groupFormationDate);

        let request = null;
        if (this.state.courseInformation?.isLocked) {
            request = {
                courseName: this.state.code + '/' + this.state.year + this.state.semester,
                description: this.state.shortDescription,

                newStudents: arr,
                newTAs: this.state.TAList,
                newinstructors: this.state.instructorList,
            };
        } else {
            request = {
                courseName: this.state.code + '/' + this.state.year + this.state.semester,
                description: this.state.shortDescription,

                newStudents: arr,
                newTAs: this.state.TAList,
                newinstructors: this.state.instructorList,

                minSize: this.state.minSize,
                maxSize: this.state.maxSize,
                groupFormationDate: date,
            };
        }

        putCourseRequest(
            this.props.match.params.courseId,
            this.state.code,
            this.state.semester,
            this.state.year,
            this.state.courseInformation,
            this.state.lockDate,
            this.state.minSize,
            this.state.maxSize
        );

        console.log(request);
    };

    deleteCourse = () => {
        deleteCourseRequest(this.props.match.params.courseId);
    };

    deactiveCourse = () => {
        postDeactivateCourseRequest(this.props.match.params.courseId);
    };

    removeUserFromCourse = (userMail) => {
        const request = {
            userMail: userMail,
            courseId: this.props.match.params.courseId,
            remove: true,
        };

        console.log(request);
    };

    ungroup = (groupId) => {
        const request = {
            groupId: groupId,
            courseId: this.props.match.params.courseId,
            ungroup: true,
        };

        console.log(request);
    };

    removeStudent = (groupId, studentId) => {
        const request = {
            groupId: groupId,
            studentId: studentId,
            remove: true,
        };

        console.log(request);
    };

    mergeGroup = (groupId, otherGroupId) => {
        const request = {
            groupId: groupId,
            otherGroupId: otherGroupId,
            merge: true,
        };

        console.log(request);
    };

    componentDidMount() {
        getCourseRequest(this.props.match.params.courseId).then((response) => {
            if (!response.data.success) return;

            const courseData = response.data?.data;

            let numberOfSections = courseData.numberOfSections;
            let studentManualList = [];
            let studentAutoList = [];
            for (let i = 0; i < numberOfSections; i++) {
                studentAutoList.push([]);
                studentManualList.push([]);
            }
            let sections = [];
            for (let i = 1; i <= numberOfSections; i++) {
                sections.push({
                    key: i,
                    text: i,
                    value: i,
                });
            }

            let date = courseData.lockDate;

            this.setState({
                settingTitle:
                    'Course Settings ' + courseData?.name + '-' + courseData?.year + courseData?.courseSemester,
                code: courseData.name,
                year: courseData.year,
                semester: courseData.courseSemester,
                shortDescription: courseData.courseInformation,
                isSectionless: courseData.isSectionless,
                //isLocked: ,
                numberOfSections: numberOfSections,
                //sections: sections,
                instructorList: [],
                currentInstructor: '',
                TAList: [],
                currentTA: '',
                studentManualList: studentManualList,
                manualSection: 1,
                currentStudent: '',
                studentAutoList: studentAutoList,
                autoSection: 1,
                groupChangeSection: 1,
                minSize: courseData.minGroupSize,
                maxSize: courseData.maxGroupSize,
                groupFormationDate: date,
                //groups: dummyGroups,
                //users: dummyUsers,
            });
        });
    }

    handleChange = (event, data) => {
        event.preventDefault();

        if (!this.validations(data)) {
            return;
        }
        const name = data.name;
        let value = '';
        value = data.value;
        this.setState({
            [name]: value,
        });
    };

    validations = (data) => {
        const name = data.name;
        switch (name) {
            case 'code':
                if (String(data.value).length > 7) return false;
                break;
            default:
                break;
        }

        return true;
    };

    removeUser = (element, listType, section) => {
        let ary = [...this.state[listType]];
        if (section === 0) {
            ary = _.without(ary, element);
        } else {
            for (var i = 0; i < this.state.sectionNumber; i++) {
                ary[i] = [...this.state[listType][i]];
            }
            ary[section - 1] = _.without(ary[section - 1], element);
        }
        this.setState({ [listType]: ary });
    };

    createUserList(members, userType, listType, section = 0) {
        let list = section === 0 ? members : members ? members[section - 1] : null;
        return (
            <Segment style={{ height: '200px' }}>
                <List selection className="UserList" items={list}>
                    {list?.map((element) => {
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
        for (let i = 0; i < this.state[userType].length; i++)
            if (
                this.state[userType][i] === '@' &&
                i + 1 < this.state[userType].length &&
                this.state[userType].indexOf('bilkent', i + 1) === -1
            ) {
                window.alert('Please enter bilkent email');
                return;
            }
        let curList = [...this.state[listType]];
        if (section === 0) {
            if (this.checkIfExists(curList, this.state[userType])) {
                window.alert("You can't add already existing user.");
                return;
            }
            curList.push(this.state[userType]);
        } else {
            for (let i = 0; i < curList.length; i++) {
                if (this.checkIfExists(curList[i], this.state[userType])) {
                    window.alert("You can't add already existing user.");
                    return;
                }
            }
            for (var i = 0; i < this.state.sectionNumber; i++) {
                curList[i] = [...this.state[listType][i]];
            }
            curList[section - 1].push(this.state[userType]);
        }
        this.setState({ [listType]: curList, [userType]: '' });
    };

    checkIfExists = (list, element) => {
        for (let i = 0; i < list.length; i++) {
            if (element === list[i]) {
                return true;
            }
        }
        return false;
    };

    readFile = (e) => {
        const reader = new FileReader();
        let students;
        reader.onload = async (file) => {
            let curList = [...this.state.studentAutoList];
            for (var i = 0; i < this.state.sectionNumber; i++) {
                curList[i] = [...this.state.studentAutoList[i]];
            }
            const text = file.target.result;
            students = text.split(/\n/);
            curList[this.state.autoSection - 1] = students;
            this.setState({ studentAutoList: curList /* currentFile: */ });
        };
        reader.readAsText(e.target.files[0]);
    };

    onKeyDown = (e) => {
        if (e.keyCode === 13) {
            e.preventDefault();
            return false;
        }
    };

    render() {
        console.log(this.state.groups, this.state.groupChangeSection);
        return (
            <>
                <Form className="SettingsForm" onSubmit={this.onFormSubmit} onKeyDown={this.onKeyDown}>
                    <Form.Group>
                        <h1>{this.state.settingTitle}</h1>
                    </Form.Group>
                    <Divider />
                    <Form.Group>
                        <Form.Field width={3}>
                            <label for="code">Code:</label>{' '}
                            <Form.Input
                                value={this.state.code}
                                onChange={this.handleChange}
                                name="code"
                                style={{ width: '100%' }}
                            />
                        </Form.Field>
                        <Form.Field width={3}>
                            <label for="year">Year:</label>
                            <Form.Input
                                value={this.state.year}
                                min="0"
                                name="year"
                                style={{ width: '100%' }}
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
                                {(this.state.code !== '' || this.state.code !== '') && '-'}
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
                            style={{ width: '100%', height: '42px' }}
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
                                            defaultValue={
                                                this.state.sections
                                                    ? this.state.sections[0]
                                                        ? this.state.sections[0].value
                                                        : 1
                                                    : 1
                                            }
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
                                            defaultValue={
                                                this.state.sections
                                                    ? this.state.sections[0]
                                                        ? this.state.sections[0].value
                                                        : 1
                                                    : 1
                                            }
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
                    {!this.state.isLocked ? (
                        <>
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
                                            type="datetime-local"
                                            name="groupFormationDate"
                                            onChange={this.handleChange}></Input>
                                    </div>
                                </Form.Field>
                            </Form.Group>
                            <Divider />
                        </>
                    ) : null}
                    <Button onClick={this.updateGeneralSettings}>Update Above Settings</Button>
                    <Button
                        color="red"
                        content="Delete Course"
                        icon="exclamation"
                        style={{ marginLeft: '10px' }}
                        onClick={this.deleteCourse}
                    />
                    <Button
                        color="red"
                        content="Deactivate Course"
                        icon="exclamation"
                        style={{ marginLeft: '10px' }}
                        onClick={this.deactiveCourse}
                    />
                    <Divider />
                    <h2>Remove Instructors/TAs/Students From Course</h2>
                    <UserSearchBar users={this.state.users} removeUserFromCourse={this.removeUserFromCourse} />
                    <Divider />
                    <h1>Change Groups</h1>
                    Section:
                    <Form.Field>
                        <Dropdown
                            fluid
                            selection
                            options={this.state.sections}
                            defaultValue={
                                this.state.sections ? (this.state.sections[0] ? this.state.sections[0].value : 1) : 1
                            }
                            name="groupChangeSection"
                            onChange={this.handleChange}
                        />
                    </Form.Field>
                    <Divider />
                    {!this.state.isLocked &&
                        convertUnformedGroupsToBriefList(
                            this.state.groups
                                ? this.state.groups[this.state.groupChangeSection - 1]
                                    ? this.state.groups[this.state.groupChangeSection - 1].formed
                                    : null
                                : null,
                            this.ungroup
                        )}
                    {this.state.isLocked && (
                        <FormedGroupsBriefList
                            removeStudent={this.removeStudent}
                            mergeGroup={this.mergeGroup}
                            ungroup={this.ungroup}
                            groups={this.state.groups ? this.state.groups[this.state.groupChangeSection - 1] : null}
                        />
                    )}
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
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            userId: 2,
        },
    ],
    TAs: [
        {
            name: 'Erdem Tuna',
            userId: 1,
        },
        {
            name: 'Kraliçe Irmak',
            userId: 2,
        },
    ],
    information:
        'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Odio, et assumenda fugiat repudiandae doloribus eaque at possimus tenetur cum ratione, non voluptatibus? Provident nam cum et cupiditate corporis earum vel ut? Illum beatae molestiae praesentium cumque sapiente, quasi neque consequatur distinctio iste possimus in dolor. Expedita rem totam ex distinctio!',
    isTAorInstructorOfCourse: true,
    isLocked: false,
    numberOfSections: 3,
    minSize: 3,
    maxSize: 5,
    groupFormationDate: new Date(2022, 1, 2, 17, 30),
};

const dummyGroups = [
    {
        formed: [
            {
                members: [
                    {
                        name: '1Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '1Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '1Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '2Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '2Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '2Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
    },
    {
        formed: [
            {
                members: [
                    {
                        name: '5Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '5Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '5Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '6Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '6Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '6Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
    },
    {
        formed: [
            {
                members: [
                    {
                        name: '9Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '9Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '9Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
            {
                members: [
                    {
                        name: '10Yusuf Uyar',
                        userId: 1,
                    },
                    {
                        name: '10Halil Özgür Demir',
                        userId: 2,
                    },
                    {
                        name: '10Barış Ogün Yörük',
                        userId: 3,
                    },
                ],
                groupId: 1,
            },
        ],
    },
];

const dummyGroupsLocked = [
    [
        {
            members: [
                {
                    name: '1Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '1Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '1Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 1,
            groupName: 'BilHub',
        },
        {
            members: [
                {
                    name: '2Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '2Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '2Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 2,
            groupName: 'Not Bilhub',
        },
    ],
    [
        {
            members: [
                {
                    name: '2Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '2Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '2Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 1,
            groupName: 'BilHub',
        },
        {
            members: [
                {
                    name: '3Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '3Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '3Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 2,
            groupName: 'Not Bilhub',
        },
    ],
    [
        {
            members: [
                {
                    name: '4Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '4Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '4Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 1,
            groupName: 'BilHub',
        },
        {
            members: [
                {
                    name: '5Yusuf Uyar',
                    userId: 1,
                },
                {
                    name: '5Halil Özgür Demir',
                    userId: 2,
                },
                {
                    name: '5Barış Ogün Yörük',
                    userId: 3,
                },
            ],
            groupId: 2,
            groupName: 'Not Bilhub',
        },
    ],
];

const dummyUsers = [
    { type: 'Student', mail: 'yusuf@bilkent.edu.tr' },
    { type: 'Student', mail: 'baris@bilkent.edu.tr' },
    { type: 'Student', mail: 'aybala@bilkent.edu.tr' },
    { type: 'Student', mail: 'ozgur@bilkent.edu.tr' },
    { type: 'Student', mail: 'ozco@bilkent.edu.tr' },
    { type: 'Student', mail: 'mert@bilkent.edu.tr' },
    { type: 'Student', mail: 'ata@bilkent.edu.tr' },
    { type: 'Student', mail: 'asd@bilkent.edu.tr' },
    { type: 'Student', mail: 'fgh@bilkent.edu.tr' },
    { type: 'Student', mail: 'jkl@bilkent.edu.tr' },
    { type: 'Student', mail: 'qwe@bilkent.edu.tr' },
    { type: 'Student', mail: 'rety@bilkent.edu.tr' },
    { type: 'Student', mail: 'werrd@bilkent.edu.tr' },
    { type: 'Student', mail: 'xcccxs@bilkent.edu.tr' },
    { type: 'Student', mail: 'bvbvbvb@bilkent.edu.tr' },
    { type: 'Student', mail: 'fdgggggw@bilkent.edu.tr' },
    { type: 'TA', mail: 'TA@bilkent.edu.tr' },
    { type: 'TA', mail: 'TA22@bilkent.edu.tr' },
    { type: 'Instructor', mail: 'hoca@bilkent.edu.tr' },
];
