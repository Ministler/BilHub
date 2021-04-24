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
var code = '';
var year = '';
var semester = '';
export class CourseCreation extends Component {
    constructor(props) {
        super(props);
        this.state = {
            code: '',
            year: '',
            semester: '',
            isSectionless: false,
            sectionNumber: 0,
            instructorList: {},
            TAList: {},
            studentList: {},
        };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event, data) {
        const name = data.name;
        var value = '';
        if (data.type === 'checkbox') {
            value = data.checked;
        } else {
            value = data.value;
        }

        this.setState({
            [name]: value,
        });
        console.log(data);
    }

    createUserList(members) {
        return (
            <Segment style={{ height: '200px' }}>
                <List items={members}></List>
                <Segment.Inline className="AddSegment">
                    {this.state.isActive && <Input style={{ width: '70%' }}></Input>}

                    <Icon className="PlusButton clickableChangeColor" size="big" name="plus"></Icon>
                </Segment.Inline>
            </Segment>
        );
    }

    render() {
        return (
            <Form>
                <Form.Group>
                    <h1>Create New Course</h1>
                </Form.Group>
                <Divider />
                <Form.Group>
                    <Form.Field width={3}>
                        <label for="code">Code:</label>{' '}
                        <Form.Input onChange={this.handleChange} name="code" style={{ width: '50%' }} />
                    </Form.Field>
                    <Form.Field width={3}>
                        <label for="year">Year:</label>
                        <Form.Input name="year" style={{ width: '50%' }} onChange={this.handleChange} type="number" />
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
                    <Form.Field width={7} textAlign="center">
                        {this.state.code}
                        {(this.state.code != '' || this.state.code != '') && '-'}
                        {this.state.year} {this.state.semester}
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
                                label="Number of Sections: "
                                name="sectionNumber"
                                onChange={this.handleChange}
                                style={{ width: '25%' }}
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
                    <Grid.Row columns={6}>
                        <GridColumn>Add Instructor:</GridColumn>
                        <GridColumn>{this.createUserList(this.state.instructorList)}</GridColumn>
                        <GridColumn>Add Teaching Assistants</GridColumn>
                        <GridColumn>{this.createUserList(this.state.instructorList)}</GridColumn>
                    </Grid.Row>
                    <Grid.Row columns={6}>
                        <GridColumn>
                            <div>Add Student as .txt file:</div>
                            <div>
                                Section: <Dropdown />
                            </div>
                        </GridColumn>
                        <GridColumn>
                            <Button>Add File</Button>
                        </GridColumn>
                        <GridColumn>
                            <div>Add Student as a list:</div>
                            <div>
                                Section: <Dropdown />
                            </div>
                        </GridColumn>
                        <GridColumn>{this.createUserList(this.state.instructorList)}</GridColumn>
                    </Grid.Row>
                </Grid>
                <Divider />
                <Grid columns={1}>
                    <GridColumn>
                        <Grid.Row>
                            Group Formation Type:<Dropdown></Dropdown>
                            <Icon name="info circle"></Icon>
                        </Grid.Row>
                        <Grid.Row>
                            Min:<Input></Input> Max:<Input></Input>
                        </Grid.Row>
                        <Grid.Row>
                            Group Formation Date
                            <Input type="date"></Input>
                        </Grid.Row>
                    </GridColumn>
                </Grid>
                <Divider />
                <Button>Create New Course</Button>
            </Form>
        );
    }
}
