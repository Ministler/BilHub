import React, { Component } from 'react';
import { Icon, Input, TextArea, Segment, Button, Grid } from 'semantic-ui-react';
import { connect } from 'react-redux';
import axios from 'axios';

import './Project.css';
import { InformationSection, NewCommentModal, EditCommentModal, DeleteCommentModal } from './ProjectComponents';
import {
    Tab,
    convertAssignmentsToAssignmentList,
    getGradeTable,
    GradePane,
    getFeedbacksAsAccordion,
    convertMembersToMemberElement,
    FeedbacksPane,
} from '../../components';
import { ProjectSubmission } from './ProjectSubmission';

class Project extends Component {
    constructor(props) {
        super(props);
        this.state = {
            projectGroup: { members: [] },
            assignments: null,
            grades: null,
            feedbacks: [],

            // States regarding changing left part of the page
            nameEditMode: false,
            informationEditMode: false,
            newName: '',
            newInformation: '',

            // States regarding open modals of right part
            isGiveFeedbackOpen: false,
            isEditFeedbackOpen: false,
            isDeleteFeedbackOpen: false,
            isFeedbackSRS: false,
            currentFeedbackText: '',
            currentFeedbackFile: null,
            currentFeedbackGrade: 10,
            currentMaxFeedbackGrade: 10,
            currentFeedbackId: 0,
        };
    }

    changeGroupName = (newName) => {
        const request = {
            newName: newName,
            groupId: this.props.match.params.projectId,
        };

        console.log(request);
    };

    changeGroupInformation = (newInformation) => {
        const request = {
            newInformation: newInformation,
            groupId: this.props.match.params.projectId,
        };

        console.log(request);
    };

    onAssignmentFileClicked = () => {
        console.log('FILE');
    };

    onModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (this.state.isFeedbackSRS) {
            if (modalType === 'isGiveFeedbackOpen') {
                request = {
                    grade: this.state.currentFeedbackGrade,
                    maxGrade: this.state.currentMaxFeedbackGrade,
                    userId: this.props.userId,
                    groupId: this.props.match.params.projectId,
                };
            } else if (modalType === 'isEditFeedbackOpen') {
                request = {
                    grade: this.state.currentFeedbackGrade,
                    maxGrade: this.state.currentMaxFeedbackGrade,
                    userId: this.props.userId,
                };
            } else if (modalType === 'isDeleteFeedbackOpen') {
                request = {
                    userId: this.props.userId,
                };
            }
        } else {
            if (modalType === 'isGiveFeedbackOpen') {
                request = {
                    newGrade: this.state.currentFeedbackGrade,
                    newText: this.state.currentFeedbackText,
                    newFile: this.state.currentFeedbackFile,
                    userId: this.props.userId,
                    groupId: this.props.match.params.projectId,
                };
            } else if (modalType === 'isEditFeedbackOpen') {
                request = {
                    newGrade: this.state.currentFeedbackGrade,
                    newText: this.state.currentFeedbackText,
                    newFile: this.state.currentFeedbackFile,
                    commentId: this.state.currentFeedbackId,
                    userId: this.props.userId,
                };
            } else if (modalType === 'isDeleteFeedbackOpen') {
                request = {
                    commentId: this.state.currentFeedbackId,
                    userId: this.props.userId,
                };
            }
        }

        console.log(request);
    };

    componentDidMount() {
        this.setState({
            projectGroup: dummyProjectGroup,
            assignments: dummyAssignmentsList,
            grades: dummyGrades,
            feedbacks: dummyFeedbacks,

            newName: dummyProjectGroup.name,
            newInformation: dummyProjectGroup.information,
        });
    }

    // LEFT SIDE LOGIC
    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    onUserClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    onInputChanged = (e, stateName) => {
        e.preventDefault();
        this.setState({
            [stateName]: e.target.value,
        });
    };

    onEditModeToggled = (editMode) => {
        this.setState((prevState) => {
            return {
                [editMode]: !prevState[editMode],
            };
        });
    };

    // RIGHT SIDE LOGIC
    onSubmissionClicked = (projectId, submissionId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionId);
    };

    onAuthorClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    // MODAL LOGIC
    onCurrentFeedbackTextChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackText: e.target.value,
        });
    };

    onCurrentFeedbackGradeChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackGrade: e.target.value,
        });
    };

    onCurrentFeedbackMaxGradeChanged = (e) => {
        e.preventDefault();
        this.setState({
            currentMaxFeedbackGrade: e.target.value,
        });
    };

    onModalOpened = (modalType, isFeedbackSRS) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modalType) {
            this.setState({
                [modalType]: true,
                currentFeedbackGrade: 10,
                currentMaxFeedbackGrade: 10,
                currentFeedbackFile: 'empty',
                currentFeedbackText: '',
            });
        }
    };

    onModalOpenedWithComment = (
        modalType,
        isFeedbackSRS,
        commentId,
        commentText,
        commentGrade,
        commentFile,
        SRSMaxGrade
    ) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
                currentMaxFeedbackGrade: SRSMaxGrade,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
                currentMaxFeedbackGrade: 10,
            });
        }

        if (modalType) {
            this.setState({
                currentFeedbackId: commentId,
                [modalType]: true,
                currentFeedbackText: commentText,
                currentFeedbackGrade: commentGrade,
                currentFeedbackFile: commentFile,
            });
        }
    };

    getInformationPart = () => {
        return (
            <InformationSection
                onCourseClicked={() => this.onCourseClicked(this.state.projectGroup.courseId)}
                courseName={this.state.projectGroup.courseName}
                groupNameElement={this.getGroupNameElement()}
                nameEditIcon={this.getNameEditIcon()}
                memberElements={convertMembersToMemberElement(this.state.projectGroup.members, this.onUserClicked)}
                informationElement={this.getGroupInformationItem()}
                informationEditIcon={this.getInformationEditIcon()}
            />
        );
    };

    getGroupNameElement = () => {
        let groupNameElement = this.state.projectGroup.name;
        if (this.state.nameEditMode) {
            groupNameElement = (
                <Input
                    className="GroupNameChangeInput"
                    onChange={(e) => this.onInputChanged(e, 'newName')}
                    value={this.state.newName}
                />
            );
        }

        return groupNameElement;
    };

    getNameEditIcon = () => {
        // EDITING GROUP NAME ELEMENTS
        let nameEditIcon = null;
        if (this.state.projectGroup.isNameChangeable && this.state.projectGroup.isInGroup) {
            nameEditIcon = this.state.nameEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeGroupName(this.state.newName);
                        this.onEditModeToggled('nameEditMode');
                    }}
                    name={'check'}
                    color="blue"
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('nameEditMode');
                    }}
                    name={'edit'}
                    color="blue"
                />
            );
        }
        return nameEditIcon;
    };

    getGroupInformationItem = () => {
        let groupInformationElement = this.state.projectGroup.information;
        if (this.state.informationEditMode) {
            groupInformationElement = (
                <TextArea
                    onChange={(e) => this.onInputChanged(e, 'newInformation')}
                    value={this.state.newInformation}
                />
            );
        }
        return groupInformationElement;
    };

    getInformationEditIcon = () => {
        // EDITING GROUP INFORMATION ELEMENTS
        let informationEditIcon = null;
        if (this.state.projectGroup.isProjectActive && this.state.projectGroup.isInGroup) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeGroupInformation(this.state.newInformation);
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'check'}
                    color="blue"
                    style={{ float: 'right', marginTop: '-10px' }}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onEditModeToggled('informationEditMode');
                    }}
                    name={'edit'}
                    color="blue"
                    style={{ float: 'right', marginTop: '-15px' }}
                />
            );
        }

        return informationEditIcon;
    };

    getAssignmentPane = () => {
        return {
            title: 'Assignments',
            content: this.state.assignments ? (
                convertAssignmentsToAssignmentList(
                    this.state.assignments,
                    null,
                    this.onSubmissionClicked,
                    this.onAssignmentFileClicked
                )
            ) : (
                <div>No Assignments</div>
            ),
        };
    };

    getGradesPane = () => {
        return {
            title: 'Grades',
            content: this.state.grades ? (
                <GradePane
                    tables={getGradeTable(
                        this.state.grades,
                        this.state.grades?.projectAverage,
                        this.state.grades?.courseAverage,
                        this.state.projectGroup?.name
                    )}
                    finalGrade={this.state.grades.finalGrade}
                />
            ) : (
                <div>No Grades</div>
            ),
        };
    };

    getNewCommentButton = () => {
        let newCommentButton = null;
        if (this.state.projectGroup?.canUserComment) {
            newCommentButton = (
                <Button
                    content="Give Feedback"
                    labelPosition="right"
                    icon="plus"
                    primary
                    onClick={() => this.onModalOpened('isGiveFeedbackOpen', false)}
                />
            );
        }

        return newCommentButton;
    };

    onFeedbackFileClicked = (feedbackId) => {
        let headers = new Headers();

        headers.append(
            'Authorization',
            'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJjYWdyaUBkdXJndXQiLCJuYmYiOjE2MTk3ODAwODUsImV4cCI6MTYxOTg2NjQ4NSwiaWF0IjoxNjE5NzgwMDg1fQ.eOGYYn_ZCDarcW2v73y-CFr8esM5cQCl4LeGdi_H08Zmwkk_Oi24FhlmQ8MRJyc6XphdLH7mao-ESzkmjun00g'
        );

        axios
            .get('https://bb63990cfdb4.ngrok.io/Comment/2', {
                headers: headers,
            })
            .then((response) => {
                console.log(response);
            });
    };

    getFeedbacksPane = () => {
        const newCommentButton = this.getNewCommentButton();
        const content = (
            <Grid>
                {/* <div class="sixteen wide column">
                    <p>Your Feedback:</p>
                    <Form reply style={{width: "95%"}}>
                        <Form.TextArea rows="5"/>
                        <Button content='Upload File' floated='left' Compact icon labelPosition='right'>Upload File<Icon name='file' /></Button>
                        <Button content='Grade Placeholder' floated='left'/>
                        <Button content='Give Feedback' primary floated='right' Compact/>
                    </Form>
                </div>
                <div class="sixteen wide column" style={{marginTop: "-20px"}}>
                    <Divider/>
                </div> */}
                <div class="sixteen wide column">
                    <FeedbacksPane
                        feedbacksAccordion={getFeedbacksAsAccordion(
                            this.state.feedbacks,
                            this.state.projectGroup?.isTAorInstructor,
                            this.onModalOpenedWithComment,
                            this.onAuthorClicked,
                            this.props.userId,
                            this.onFeedbackFileClicked,
                            this.onModalOpened
                        )}
                    />
                </div>
                {newCommentButton}
            </Grid>
        );
        return {
            title: 'Feedbacks',
            content: content,
        };
    };

    getPaneElements = () => {
        return [this.getAssignmentPane(), this.getGradesPane(), this.getFeedbacksPane()];
    };

    // Modals
    getModals = () => {
        return (
            <>
                <NewCommentModal
                    isOpen={this.state.isGiveFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isGiveFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                    onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isEditFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChanged(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChanged(e)}
                    onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.state.projectGroup.name}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    maxGrade={this.state.currentMaxFeedbackGrade}
                />
            </>
        );
    };

    render() {
        return (
            <div class="ui centered grid">
                <Grid.Row divided>
                    <div class="four wide column">
                        <Segment style={{ boxShadow: 'none', border: '0' }}>{this.getInformationPart()}</Segment>
                    </div>
                    <div class="twelve wide column">
                        {!this.props.match.params.submissionId ? (
                            <Tab tabPanes={this.getPaneElements()} />
                        ) : (
                            <ProjectSubmission
                                projectName={this.state.projectGroup?.name}
                                projectId={this.props.match.params.projectId}
                                submissionId={this.props.match.params.submissionId}
                                userId={this.props.userId}
                            />
                        )}
                    </div>
                </Grid.Row>
                {this.getModals()}
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userName: state.name,
        userType: state.userType,
        userId: state.userId,
    };
};

export default connect(mapStateToProps)(Project);

const dummyProjectGroup = {
    isInGroup: true,
    isTAorInstructor: true,
    canUserComment: true,
    name: 'BilHub',
    isNameChangeable: true,
    isProjectActive: true,
    courseName: 'CS319-2021Spring',
    courseId: 1,
    members: [
        { name: 'Barış Ogün Yörük', userId: 1 },
        { name: 'Halil Özgür Demir', userId: 2 },
        { name: 'Yusuf Uyar Miraç', userId: 3 },
        { name: 'Aybala Karakaya', userId: 4 },
        { name: 'Çağrı Mustafa Durgut', userId: 5 },
        { name: 'Oğuzhan Özçelik', userId: 6 },
    ],
    information:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Deleniti reiciendis quae provident, fugit animi in tempore laudantium doloribus ipsum repellat voluptates ullam corporis dignissimos, porro sit optio culpa laboriosam explicabo consequuntur cum adipisci rerum quibusdam quas debitis. Nemo error accusantium tempora. Nisi autem, ipsum laboriosam aperiam quam harum debitis doloremque.',
};

const dummyAssignmentsList = [
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'graded',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 1,
        submissionId: 1,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'notsubmitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 2,
        submissionId: 2,
        file: 'dummyFile',
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionId: 3,
    },
    {
        title: 'CS319-2021Spring / Desing Report Assignment',
        status: 'submitted',
        caption:
            'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
        publisher: 'Erdem Tuna',
        publishmentDate: '13 March 2023 12:00',
        dueDate: '16 April 2025, 23:59',
        projectId: 3,
        submissionId: 3,
    },
];

const dummyGrades = {
    persons: [
        {
            name: 'Eray Tüzün',
            type: 'Project Instructor',
            grade: '9.5',
        },
        {
            name: 'Alper Sarıkan',
            type: 'Instructor',
            grade: '8.1',
        },
        {
            name: 'Erdem Tuna',
            type: 'TA',
            grade: '7.1',
        },
        {
            name: 'Elgun Jabrayilzade',
            type: 'TA',
            grade: '8.1',
        },
    ],
    studentsAverage: 8,
    projectAverage: 7.1,
    courseAverage: 6.5,
    finalGrade: 38,
};

const dummyFeedbacks = {
    // SRSResult: {
    //     grade: '9.5',
    //     maxGrade: '11',
    // },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 3,
            userId: 'dD3wUcJiDHTM9aDs8livI9HpY3h2',
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            commentId: 3,
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lcaptionorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 5,
            userId: 1,
            userGroupName: 'ClassRoom Helper',
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
            userGroupName: 'ProjectManager',
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
