import React, { Component } from 'react';
import { Icon, Dropdown, Button, Accordion } from 'semantic-ui-react';
import { withRouter } from 'react-router-dom';

import './CourseAssignment.css';
import { AssignmentCardElement, Tab, getSubmissionsAsAccordion, getAssignmentStatistics } from '../../../components';

class CourseAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            assignment: null,
            submissions: null,
            currentSection: 0,
        };
    }

    onSubmissionFileClicked = () => {
        console.log('CLICKED');
    };

    onFileClicked = () => {
        console.log('File');
    };

    onDownloadAllFiles = () => {
        console.log('File');
    };

    onDownloadNotGradedFiles = () => {
        console.log('file');
    };

    componentDidMount() {
        this.setState({
            assignment: dummyAssignment,
            submissions: dummyGroupSubmissions,
            currentSection: dummyAssignment.currentUserSection ? dummyAssignment.currentUserSection - 1 : 0,
        });
    }

    onReturnProjectPage = () => {
        this.props.history.replace('/course/' + this.props.match.params.courseId);
    };

    getAssignmentControlIcons = () => {
        let controlIcons = null;
        if (this.props.isCourseActive && this.state.assignment?.isUserTAorInstructor) {
            controlIcons = (
                <>
                    <Icon
                        name="close"
                        color="red"
                        style={{ float: 'right' }}
                        onClick={this.props.onDeleteAssignmentModalOpened}
                    />
                    <Icon
                        name="edit"
                        color="blue"
                        style={{ float: 'right' }}
                        onClick={this.props.onEditAssignmentModalOpened}
                    />
                </>
            );
        }

        return controlIcons;
    };

    getAssignmentPane = () => {
        return {
            title: 'Assignment Information',
            content: (
                <AssignmentCardElement
                    title={this.props.courseName + ' / ' + this.state.assignment?.title}
                    caption={this.state.assignment?.caption}
                    publisher={this.state.assignment?.publisher}
                    fileIcon={this.state.assignment?.file ? <Icon name="file" color="grey" /> : null}
                    fileClicked={this.onFileClicked}
                    date={
                        'Publishment Date: ' +
                        this.state.assignment?.publishmentDate +
                        ' / ' +
                        'Due Date: ' +
                        this.state.assignment?.dueDate
                    }
                    titleIcon={this.getAssignmentControlIcons()}
                />
            ),
        };
    };

    onSectionChanged = (dropdownValues) => {
        this.setState({
            currentSection: dropdownValues.value,
        });
    };

    getDropdownForSection = () => {
        if (this.state.assignment?.numberOfSections) {
            const sectionOptions = [];
            for (let i = 0; i < this.state.assignment?.numberOfSections; i++) {
                sectionOptions.push({
                    key: i,
                    text: 'Section ' + (i + 1),
                    value: i,
                });
            }
            return (
                <Dropdown
                    selection
                    options={sectionOptions}
                    value={this.state.currentSection}
                    onChange={(e, dropdownValues) => this.onSectionChanged(dropdownValues)}
                />
            );
        }

        return null;
    };

    onSubmissionPageClicked = (projectId, submissionId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionId);
    };

    getSubmissionsPane = () => {
        return {
            title: 'Submissions',
            content: (
                <>
                    {this.getDropdownForSection()}
                    {this.state.submissions?.length >= 1 &&
                    this.state.currentSection < this.state.submissions?.length &&
                    0 <= this.state.submissions?.length
                        ? getSubmissionsAsAccordion(
                              this.state.submissions[this.state.currentSection],
                              this.onSubmissionPageClicked,
                              this.onSubmissionFileClicked
                          )
                        : null}
                    {this.getFileDownloadButtons()}
                </>
            ),
        };
    };

    getStatisticsPane = () => {
        return {
            title: 'Statistics',
            content: <>{getAssignmentStatistics(dummyAssignmentGrades, dummyFinalGrades)}</>,
        };
    };

    getPaneElements = () => {
        return [this.getAssignmentPane(), this.getSubmissionsPane(), this.getStatisticsPane()];
    };

    getFileDownloadButtons = () => {
        let buttons = null;
        if (this.state.assignment?.isUserTAorInstructor) {
            buttons = (
                <>
                    <Button icon={'download'} onClick={this.onDownloadAllFiles}>
                        Download All Files
                    </Button>
                    <Button icon={'download'} onClick={this.onDownloadNotGradedFiles}>
                        Donwload Only Not Graded Files
                    </Button>
                </>
            );
        }
        return buttons;
    };

    render() {
        return (
            <div class="inline">
                <Icon
                    onClick={this.onReturnProjectPage}
                    size="big"
                    name="angle left"
                    color="blue"
                    style={{ display: 'inline' }}
                />
                <p
                    onClick={this.onReturnProjectPage}
                    style={{ display: 'inline', fontSize: '16px', fontWeight: 'bold', color: 'rgb(33, 133, 208)' }}>
                    Back To Course Page
                </p>
                <Tab tabPanes={this.getPaneElements()} />
            </div>
        );
    }
}

const dummyAssignment = {
    title: 'Analysis Report',
    caption:
        'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Pariatur optio dolores modi illo, soluta nesciunt? Explicabo dicta ad nulla ea.',
    publisher: 'Elgun Jabrayilzade',
    file: 'file',
    isUserTAorInstructor: true,
    publishmentDate: '12 March 2021 12:00',
    dueDate: '12 March 2021 12:00',
    numberOfSections: 3,
    currentUserSection: 2,
};

const dummyGroupSubmissions = [
    {
        graded: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionId: 2,
            },
        ],
        notSubmitted: [
            {
                groupName: 'BilHub',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                projectId: 2,
                submissionId: 2,
            },
        ],
    },
    {
        graded: [
            {
                groupName: 'BilHub2',
                fileName: '21_1_analysisReport.pdf',
                file: '2file',
                grade: '2/10',
                submissionDate: '2 March 2021',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionId: 2,
            },
        ],
        notSubmitted: [
            {
                groupName: 'BilHub',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                projectId: 2,
                submissionId: 2,
            },
        ],
    },
    {
        graded: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionId: 2,
            },
        ],
        notSubmitted: [
            {
                groupName: 'BilHub',
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                projectId: 2,
                submissionId: 2,
            },
        ],
    },
];

const dummyAssignmentGrades = {
    graders: ['Eray Tüzün', 'Alper Sarıkan', 'Erdem Tuna', 'Kraliçe Irmak', 'Students'],
    groups: [
        { name: 'BilHub', grades: [99, 98, 97, 10, 89] },
        { name: 'BilCalendar', grades: [75, 45, 23, 10, 89] },
        { name: 'CS315Odevi', grades: [38, 98, 97, 1, 43] },
        { name: 'Website', grades: [46, 87, 24, 10, 94] },
    ],
};

const dummyFinalGrades = [
    { group: 'BilHub', grade: 90 },
    { group: 'BilHub2', grade: 20 },
    { group: 'BilHub3', grade: 80 },
    { group: 'BilHubNot', grade: 78 },
    { group: 'OZCO1000', grade: 80 },
    { group: 'BilCalendar', grade: 78 },
    { group: 'Yusuf Keke', grade: 80 },
    { group: 'Website', grade: 78 },
];

export default withRouter(CourseAssignment);
