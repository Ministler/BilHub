import React, { Component } from 'react';
import { Grid, GridColumn } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Notifications.css';
import { ProfilePrompt, BriefList } from '../../commonComponents';

class Notifications extends Component {
    constructor(props) {
        super(props);
        this.state = {
            projects: null,
            instructorCourses: null,
        };
    }

    componentDidUpdate() {
        this.setState({
            projects: dummyMyProjectsList,
            instructedCourses: dummyInstructedCoursesList,
        });
    }

    onProjectClicked = (projectId) => {
        this.props.history.push('/project/' + projectId);
    };

    onInsturctedCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    render() {
        return (
            <Grid>
                <GridColumn width={4}>
                    <div className={'HomeDivLeft'}>
                        <ProfilePrompt name={this.props.name} onClick={this.onProfilePromptClicked} />
                    </div>
                </GridColumn>
                <GridColumn width={1} />
                <GridColumn width={12}></GridColumn>
            </Grid>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userType: state.userType,
        name: state.name,
        userId: state.userId,
        token: state.token,
    };
};

export default connect(mapStateToProps)(Notifications);

const dummyMyProjectsList = [
    {
        courseCode: 'CS319-2021Spring',
        projectName: 'BilHub',
        isActive: true,
        projectId: 1,
    },
    {
        courseCode: 'CS315-2021Spring',
        projectName: 'AGA',
        isActive: true,
        projectId: 2,
    },
    {
        courseCode: 'CS102-2019Fall',
        projectName: 'BilCalendar',
        isActive: false,
        projectId: 3,
    },
];

const dummyInstructedCoursesList = [
    {
        courseCode: 'CS102-2021Spring',
        isActive: true,
        courseId: 1,
    },
    {
        courseCode: 'CS102-2021Fall',
        isActive: false,
        courseId: 2,
    },
    {
        courseCode: 'CS102-2021Fall',
        isActive: false,
        courseId: 3,
    },
];
