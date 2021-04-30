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
    Icon,
    Form,
    Message,
    List,
    Popup,
    TextArea,
} from 'semantic-ui-react';

import './CourseCreation.css';

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
    {
        key: 'By Hard Coded',
        text: 'By Hard Coded',
        value: 'By Hard Coded',
    },
];

export class CourseCreation extends Component {
    constructor(props) {
        super(props);
        this.state = {
            //course info
            code: '',
            year: '',
            semester: '',
            shortDescription: '',

            //section settings
            isSectionless: false,
            sectionNumber: 1,
            sections: [
                {
                    key: 1,
                    text: 1,
                    value: 1,
                },
            ],

            //users
            instructorList: [],
            currentInstructor: '',
            TAList: [],
            currentTA: '',
            studentManualList: [[]],
            manualSection: 1,
            currentStudent: '',
            studentAutoList: [[]],
            autoSection: 1,
            currentFile: '',

            //group formation
            minSize: 1,
            maxSize: 1,
            groupFormationDate: '',
        };
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

    handleChange = (event, data) => {
        event.preventDefault();

        if (!this.validations(data)) {
            return;
        }
        const name = data.name;
        var value = '';
        if (data.type === 'checkbox') {
            value = data.checked;
            this.setState({
                [name]: value,
                sectionNumber: 1,
                sections: [
                    {
                        key: 1,
                        text: 1,
                        value: 1,
                    },
                ],
                manualSection: 1,
                autoSection: 1,
                studentAutoList: [[]],
                studentManualList: [[]],
            });
        } else {
            value = data.value;
            this.setState({
                [name]: value,
            });
        }
    };

    changeSection = (event, data) => {
        var sections = [];
        var studentManualList = [];
        var studentAutoList = [];
        for (var i = 1; i <= data.value; i++) {
            sections.push({
                key: i,
                text: i,
                value: i,
            });
            studentManualList.push([]);
            studentAutoList.push([]);
        }
        this.state.studentAutoList = studentAutoList;
        this.state.studentManualList = studentManualList;
        this.state.sections = sections;
        this.handleChange(event, data);
    };

    createUserList(members, userType, listType, section = 0) {
        var list = section === 0 ? members : members[section - 1];
        return (
            <Segment style={{ height: '200px' }}>
                <List className="UserList" items={list}></List>
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
        let curList = this.state[listType];
        if (section === 0) {
            curList.push(this.state[userType]);
        } else {
            curList[section - 1].push(this.state[userType]);
        }
        this.setState({ [listType]: curList, [userType]: '' });
    };

    onFormSubmit = (e) => {
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

    render() {
        return (
            <Form className="CreationForm" onSubmit={this.onFormSubmit}>
                <Form.Group>
                    <h1>Create New Course</h1>
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
                            max="9999"
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
                <Form.Group widths={3}>
                    <Form.Field>
                        <Checkbox
                            name="isSectionless"
                            onChange={this.handleChange}
                            label="Group Formation Between Sections"
                        />
                    </Form.Field>
                    <Form.Field>
                        {this.state.isSectionless || (
                            <Form.Input
                                type="number"
                                min="1"
                                label="Number of Sections: "
                                name="sectionNumber"
                                onChange={this.changeSection}
                                style={{ width: '25%' }}
                                value={this.state.sectionNumber}
                            />
                        )}
                    </Form.Field>
                    <Form.Field verticalAlign="middle">
                        <Message negative>
                            <Message.Header>THESE CHOICES CANNOT BE CHANGED</Message.Header>
                        </Message>
                    </Form.Field>
                </Form.Group>
                <Divider />
                <Grid>
                    <Grid.Row columns={4}>
                        <GridColumn>Add Instructor:</GridColumn>
                        <GridColumn>
                            {this.createUserList(this.state.instructorList, 'currentInstructor', 'instructorList')}
                        </GridColumn>
                        <GridColumn>Add Teaching Assistants</GridColumn>
                        <GridColumn>{this.createUserList(this.state.TAList, 'currentTA', 'TAList')}</GridColumn>
                    </Grid.Row>
                    <Grid.Row columns={4}>
                        <GridColumn>
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
                        <GridColumn>
                            <input
                                className="FileInput"
                                type="file"
                                accept=".txt"
                                value={this.state.currentFile}
                                onChange={(e) => this.readFile(e)}
                            />
                        </GridColumn>
                        <GridColumn>
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
                        <GridColumn>
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
                            <Input type="date" name="groupFormationDate" onChange={this.handleChange}></Input>
                        </div>
                    </Form.Field>
                </Form.Group>
                <Divider />
                <Button>Create New Course</Button>
            </Form>
        );
    }
}

const dummyCourse = {
    students: [
        ['ali', 'yusuf', 'ozco'],
        ['aybala', 'ozgur', 'baris', 'cagri'],
    ],
    TAs: ['eray', 'alper'],
    instructors: ['irmak', 'elgun'],
};
