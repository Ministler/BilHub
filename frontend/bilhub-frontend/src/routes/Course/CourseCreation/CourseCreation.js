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
            code: '',
            year: '',
            semester: '',

            isSectionless: false,
            sectionNumber: 1,
            sections: [
                {
                    key: 1,
                    text: 1,
                    value: 1,
                },
            ],
            instructorList: [],
            currentInstructor: '',
            TAList: [],
            currentTA: '',
            studentManualList: [],
            manualSection: 0,
            currentStudent: '',
            studentAutoList: [],
            autoSection: 0,
            groupFormationType: '',
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
            var sectionNumber = value ? 0 : 1;
            this.setState({ [name]: value, sectionNumber: sectionNumber });
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
        return (
            <Segment style={{ height: '200px' }}>
                <List className="UserList" items={members}></List>
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
                        placeholder="Enter"
                    />
                </Segment.Inline>
            </Segment>
        );
    }

    addUser = (userType, listType, section) => {
        let curList = this.state[listType];
        console.log(listType);
        if (section === 0) {
            curList.push(this.state[userType]);
        } else {
            curList[section - 1].push(this.state[userType]);
        }
        this.setState({ [listType]: curList, [userType]: '' });
    };

    onFormSubmit = (e) => {
        console.log(this.state.sectionNumber);
        return {
            courseName: this.state.code + '/' + this.state.year + this.state.semester,
            isSectionless: this.state.isSectionless,
            numberOfSection: this.state.sectionNumber, // If sectionless this field is 0

            groupFormationType: this.state.groupFormationType,
        };
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
                            {this.state.sectionNumber > 0 && (
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
                            <Button>Add File</Button>
                        </GridColumn>
                        <GridColumn>
                            <div>Add Student as a list:</div>
                            {this.state.sectionNumber > 0 && (
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
                        Group Formation Type:
                        <Dropdown
                            style={{ float: 'left' }}
                            name="groupFormationType"
                            onChange={this.handleChange}
                            fluid
                            selection
                            options={groupFormationSettings}></Dropdown>
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
                        {this.state.groupFormationType == 'By Hard Coded' && (
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
                <Button>Create New Course</Button>
            </Form>
        );
    }
}
