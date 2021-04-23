import React, { Component } from 'react';
import { Divider, Input, Dropdown } from 'semantic-ui-react';

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

export class CourseCreation extends Component {
    render() {
        return (
            <div>
                <h1>Create New Course</h1>
                <Divider />
                Code: <Input style={{ width: '5%' }} />
                Year: <Input style={{ width: '5%' }} />
                Semester:{' '}
                <Dropdown
                    as="span"
                    style={{ width: '5%' }}
                    placeholder="Select Semester"
                    fluid
                    selection
                    options={semesterOptions}
                />
            </div>
        );
    }
}
