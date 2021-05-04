import React, { Component } from 'react';
import { Icon, Dropdown, Button } from 'semantic-ui-react';
import { withRouter, Link } from 'react-router-dom';

import {
    getAssignmentRequest,
    getAssignmentFileRequest,
    getSubmissionRequest,
    getAssignmentStatisticsRequest,
    getSubmissionFileRequest,
} from '../../../API';
import './CourseAssignment.css';
import { AssignmentCardElement, Tab, getSubmissionsAsAccordion, getAssignmentStatistics } from '../../../components';
import { dateObjectToString } from '../../../utils';
import axios from 'axios';

class CourseAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            assignment: null,
            submissions: null,
            currentSection: 0,
        };
    }

    onSubmissionFileClicked = (submissionId, fileName) => {
        getSubmissionFileRequest(submissionId, fileName);
    };

    onFileClicked = () => {
        getAssignmentFileRequest(this.props.match.params.assignmentId, this.state.assignment.fileName);
    };

    onDownloadAllFiles = () => {
        console.log('File');
    };

    onDownloadNotGradedFiles = () => {
        console.log('file');
    };

    componentDidMount() {
        getAssignmentRequest(this.props.match.params.assignmentId).then((response) => {
            if (!response.data.success) return;

            const data = response.data.data;

            const assignment = {
                title: data.title,
                caption: data.assignmentDescription,
                publisher: data.publisher,
                hasFile: data.hasFile,
                fileName: data.fileName,
                isUserTAorInstructor: this.props.isUserTAorInstructor,
                publishmentDate: data.createdAt,
                dueDate: data.dueDate,
                currentUserSection: this.props.currentUserSection,
                numberOfSections: this.props.numberOfSections,
            };

            this.setState({
                assignment: assignment,
            });

            const submissionIds = data.submissionIds;
            const requests = [];
            for (let id of submissionIds) {
                requests.push(getSubmissionRequest(id));
            }

            axios.all(requests).then(
                axios.spread((...responses) => {
                    const submission = [];
                    for (let i = 0; i < this.props.numberOfSections; i++) {
                        submission.push({
                            graded: [],
                            submitted: [],
                            notSubmitted: [],
                        });
                    }
                    for (let response of responses) {
                        const data = response.data.data;
                        console.log(data);
                        if (!data.hasSubmission) {
                            submission[data.sectionNumber - 1].notSubmitted.push({
                                groupName: data.affiliatedGroup.name,
                                projectId: data.affiliatedGroup.id,
                                submissionId: data.id,
                            });
                        } else if (!data.isGraded) {
                            console.log(submission);
                            submission[data.sectionNumber - 1].submitted.push({
                                groupName: data.affiliatedGroup.name,
                                projectId: data.affiliatedGroup.id,
                                submissionId: data.id,
                                hasFile: data.hasFile,
                                submissionDate: data.updatedAt,
                                fileName: data.fileName,
                            });
                        } else {
                            submission[data.sectionNumber - 1].graded.push({
                                groupName: data.affiliatedGroup.name,
                                projectId: data.affiliatedGroup.id,
                                submissionId: data.id,
                                hasFile: data.hasFile,
                                submissionDate: data.updatedAt,
                                grade: data.srsGrade,
                                fileName: data.fileName,
                            });
                        }
                    }
                    console.log(submission);
                    this.setState({
                        submissions: submission,
                    });
                })
            );
        });

        getAssignmentStatisticsRequest(this.props.match.params.assignmentId).then((response) => {
            if (!response.data.success) return;
            console.log(response.data.data);
        });

        this.setState({
            currentSection: this.props.currentUserSection ? this.props.currentUserSection - 1 : 0,
        });
    }

    onReturnProjectPage = () => {
        this.props.history.replace('/course/' + this.props.match.params.courseId);
    };

    getAssignmentControlIcons = () => {
        let controlIcons = null;
        if (this.props.isTAorInstructorOfCourse) {
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
                    fileIcon={this.state.assignment?.hasFile ? <Icon name="file" color="grey" /> : null}
                    fileClicked={this.onFileClicked}
                    date={
                        'Publishment Date: ' +
                        (typeof this.state.assignment?.publishmentDate === 'object'
                            ? dateObjectToString(this.state.assignment?.publishmentDate)
                            : this.state.assignment?.publishmentDate) +
                        ' / ' +
                        'Due Date: ' +
                        (typeof this.state.assignment?.dueDate === 'object'
                            ? dateObjectToString(this.state.assignment?.dueDate)
                            : this.state.assignment?.dueDate)
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
            content: 'Not Implemented Yet' /*<>{getAssignmentStatistics(dummyAssignmentGrades, dummyFinalGrades)}</>*/,
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
                    <Button color="green" compact onClick={this.onDownloadAllFiles} icon labelPosition="right">
                        Download All Files <Icon name="download" />
                    </Button>
                    <Button color="green" compact onClick={this.onDownloadNotGradedFiles} icon labelPosition="right">
                        Donwload Only Not Graded Files <Icon name="download" />
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
                <Link
                    onClick={this.onReturnProjectPage}
                    style={{ display: 'inline', fontSize: '16px', fontWeight: 'bold', color: 'rgb(33, 133, 208)' }}>
                    Back To Course Page
                </Link>
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
    hasFile: 'file',
    isUserTAorInstructor: true,
    publishmentDate: new Date(2021, 3, 12, 12, 0),
    dueDate: new Date(2021, 3, 12, 12, 0),
    numberOfSections: 3,
    currentUserSection: 2,
};

const dummyGroupSubmissions = [
    {
        graded: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                grade: '7/10',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                grade: '7/10',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 2,
                submissionId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                submissionDate: new Date(2021, 3, 15, 17, 0),
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
                hasFile: '2file',
                grade: '2/10',
                submissionDate: new Date(2021, 3, 2, 17, 0),
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                grade: '7/10',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 2,
                submissionId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                submissionDate: new Date(2021, 3, 15, 17, 0),
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
                hasFile: 'file',
                grade: '7/10',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                grade: '7/10',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 2,
                submissionId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                submissionDate: new Date(2021, 3, 15, 17, 0),
                projectId: 1,
                submissionId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                hasFile: 'file',
                submissionDate: new Date(2021, 3, 15, 17, 0),
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
