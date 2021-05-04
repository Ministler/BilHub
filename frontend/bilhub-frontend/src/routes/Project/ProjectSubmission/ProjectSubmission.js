import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Icon, Button, Divider, Grid } from 'semantic-ui-react';
import { withRouter, Link } from 'react-router-dom';
import {
    NewCommentModal,
    NewCommentModal2,
    EditCommentModal,
    DeleteCommentModal,
    NewSubmissionModal,
    EditSubmissionModal,
    DeleteSubmissionModal,
} from '../ProjectComponents';
import {
    Tab,
    getGradeTable,
    getFeedbacksAsAccordion,
    GradePane,
    FeedbacksPane,
    SubmissionPane,
} from '../../../components';
import { dateObjectToString, inputDateToDateObject } from '../../../utils';
import {
    getSubmissionRequest,
    postCommentRequest,
    postSubmissionRequest,
    putSubmissionRequest,
    deleteSubmissionRequest,
    getAssignmentFileRequest,
    deleteSubmissionSrsGradeRequest,
    postSubmissionSrsGradeRequest,
} from '../../../API';
import {
    getSubmissionFileRequest,
    getSubmissionInstructorCommentsRequest,
    getSubmissionSrsGradeRequest,
    getSubmissionStudentCommentsRequest,
    getSubmissionTACommentsRequest,
} from '../../../API/submissionAPI/submissionGET';
import axios from 'axios';
import { deleteCommentRequest, putCommentRequest } from '../../../API/commentAPI';
import { getIsUserInstructorOfGroupRequest } from '../../../API/projectGroupAPI/projectGroupGET';

class ProjectAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            assignment: {},
            submissionPage: {},
            submission: null,
            grades: null,
            feedbacks: [],

            // State modals regarding submission
            isAddSubmissionOpen: false,
            isEditSubmissionOpen: false,
            isDeleteSubmissionOpen: false,
            submissionFile: null,
            submissionCaption: '',

            // States modals regarding feedbacks
            isGiveFeedbackOpen: false,
            isEditFeedbackOpen: false,
            isDeleteFeedbackOpen: false,
            isFeedbackSRS: false,
            currentFeedbackText: '',
            currentFeedbackFile: null,
            currentFeedbackGrade: 10,
            currentMaxFeedbackGrade: 10,
            currentFeedbackId: 0,

            //testing
            currentFeedbackText2: '',
            currentFeedbackGrade2: 10,
            currentMaxFeedbackGrade2: 10,
        };
    }

    onCurrentFeedbackTextChanged2 = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackText2: e.target.value,
        });
    };
    onCurrentFeedbackGradeChanged2 = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackGrade2: e.target.value,
        });
    };
    onCurrentFeedbackMaxGradeChanged2 = (e) => {
        e.preventDefault();
        this.setState({
            currentMaxFeedbackGrade2: e.target.value,
        });
    };
    onGiveFeedback = (e) => {
        postCommentRequest(
            null,
            this.state.submission.submissionId,
            this.state.currentFeedbackText2,
            this.state.currentMaxFeedbackGrade2,
            this.state.currentFeedbackGrade2
        );
    };

    onAssignmentFileClicked = () => {
        getAssignmentFileRequest(this.state.assignment.id, this.state.assignment.fileName);
    };

    onSubmissionFileClicked = () => {
        getSubmissionFileRequest(this.state.submission.submissionId);
    };

    onSubmissionFileChange = (file) => {
        this.setState({
            submissionFile: file,
        });
    };

    onSubmissionModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (modalType === 'isAddSubmissionOpen') {
            postSubmissionRequest(
                this.state.submissionFile,
                this.state.submissionCaption,
                this.props.match.params.submissionId
            );
        } else if (modalType === 'isEditSubmissionOpen') {
            putSubmissionRequest(
                this.state.submissionFile,
                this.state.submissionCaption,
                this.state.submission.submissionId
            );
        } else if (modalType === 'isDeleteSubmissionOpen') {
            deleteSubmissionRequest(this.state.submission.submissionId);
        }

        console.log(request);
    };

    onModalClosed = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;

        let request = 'error';
        if (this.state.isFeedbackSRS) {
            if (modalType === 'isGiveFeedbackOpen') {
                postSubmissionSrsGradeRequest(this.props.match.params.submissionId, this.state.currentFeedbackGrade);
            } else if (modalType === 'isEditFeedbackOpen') {
                postSubmissionSrsGradeRequest(this.props.match.params.submissionId, this.state.currentFeedbackGrade);
            } else if (modalType === 'isDeleteFeedbackOpen') {
                deleteSubmissionSrsGradeRequest(this.state.submission.submissionId);
            }
        } else {
            if (modalType === 'isGiveFeedbackOpen') {
                postCommentRequest(
                    this.state.currentFeedbackFile,
                    this.submission.submissionId,
                    this.state.currentFeedbackText,
                    10,
                    this.state.currentFeedbackGrade
                );
            } else if (modalType === 'isEditFeedbackOpen') {
                putCommentRequest(
                    this.state.currentFeedbackFile,
                    this.state.currentFeedbackId,
                    this.state.currentFeedbackText,
                    10,
                    this.state.currentFeedbackGrade
                );
            } else if (modalType === 'isDeleteFeedbackOpen') {
                deleteCommentRequest(this.state.currentFeedbackId);
            }
        }

        console.log(request);
    };

    componentDidMount() {
        getIsUserInstructorOfGroupRequest(this.props.match.params.projectId, this.props.userId).then((auth) => {
            getSubmissionRequest(this.props.match.params.submissionId).then((response) => {
                if (!response.data.success) return;
                const curSubmission = response.data.data;
                console.log(curSubmission);

                let status;
                if (curSubmission.isGraded) {
                    status = 2;
                } else if (curSubmission.hasSubmission) {
                    status = 1;
                } else {
                    status = 0;
                }
                const assignment = {
                    title: curSubmission.affiliatedAssignment.title,
                    id: curSubmission.affiliatedAssignment.id,
                    status: status,
                    hasFile: curSubmission.affiliatedAssignment.hasFile,
                    fileName: curSubmission.affiliatedAssignment.fileName,

                    caption: curSubmission.affiliatedAssignment.assignmentDescription,
                    publisher: curSubmission.affiliatedAssignment.publisher,
                    publishmentDate: inputDateToDateObject(curSubmission.affiliatedAssignment.createdAt),
                    dueDate: inputDateToDateObject(curSubmission.affiliatedAssignment.dueDate),
                    submissionInfo: '',
                };
                const submission = {
                    caption: curSubmission.description,
                    hasFile: curSubmission.hasFile,
                    date: inputDateToDateObject(curSubmission.updatedAt),
                    submissionId: curSubmission.id,
                };
                let isInGroup = false;
                for (let i of curSubmission.affiliatedGroup.groupMembers) {
                    if (i.id === this.props.userId) {
                        isInGroup = true;
                        break;
                    }
                }
                const page = {
                    isSubmissionAnonim: !curSubmission.affiliatedAssignment.visibilityOfSubmission,
                    isInGroup: true,
                    isTAorInstructor: auth.data.data,
                    canUserComment: curSubmission.affiliatedAssignment.canBeGradedByStudents,
                    hasSubmission: curSubmission.hasSubmission,
                    isLate: submission.date > assignment.dueDate,
                    isEdittable: true, //ask
                };
                this.setState({ assignment: assignment, submissionPage: page, submission: submission });
                console.log(this.state.submission?.submissionId);
                feedbackRequests.push(getSubmissionInstructorCommentsRequest(this.state.submission?.submissionId));
                feedbackRequests.push(getSubmissionTACommentsRequest(this.state.submission?.submissionId));
                feedbackRequests.push(getSubmissionStudentCommentsRequest(this.state.submission?.submissionId));
                //feedbackRequests.push(getSubmissionSrsGradeRequest()) graderi nasil alirim dusun
                const feedbacks = { InstructorComments: [], TAComments: [], StudentComments: [] };
                axios.all(feedbackRequests).then(
                    axios.spread((...responses) => {
                        let instGrade = 0;
                        for (let i in responses[0].data.data) {
                            feedbacks.InstructorComments.push({
                                name: responses[0].data.data[i].commentedUser.name,
                                caption: responses[0].data.data[i].commentText,
                                grade: responses[0].data.data[i].grade,
                                date: inputDateToDateObject(responses[0].data.data[i].createdAt),
                                commentId: responses[0].data.data[i].id,
                                userId: responses[0].data.data[i].commentedUser.id,
                            });
                            persons.push({
                                name: responses[0].data.data[i].commentedUser.name,
                                type: responses[0].data.data[i].commentedUser.userType,
                                grade: responses[0].data.data[i].grade,
                                userId: responses[0].data.data[i].commentedUser.id,
                            });
                            instGrade += responses[0].data.data[i].grade;
                        }
                        for (let i in responses[1].data.data) {
                            feedbacks.TAComments.push({
                                name: responses[1].data.data[i].commentedUser.name,
                                caption: responses[1].data.data[i].commentText,
                                grade: responses[1].data.data[i].grade,
                                date: inputDateToDateObject(responses[1].data.data[i].createdAt),
                                commentId: responses[1].data.data[i].id,
                                userId: responses[1].data.data[i].commentedUser.id,
                            });
                            persons.push({
                                name: responses[1].data.data[i].commentedUser.name,
                                type: responses[1].data.data[i].commentedUser.userType,
                                grade: responses[1].data.data[i].grade,
                                userId: responses[1].data.data[i].commentedUser.id,
                            });
                            instGrade += responses[1].data.data[i].grade;
                        }
                        let studentAvg = 0;
                        for (let i in responses[2].data.data) {
                            feedbacks.StudentComments.push({
                                name: responses[2].data.data[i].commentedUser.name,
                                caption: responses[2].data.data[i].commentText,
                                grade: responses[2].data.data[i].grade,
                                date: inputDateToDateObject(responses[2].data.data[i].createdAt),
                                commentId: responses[2].data.data[i].id,
                                userId: responses[2].data.data[i].commentedUser.id,
                            });
                            studentAvg += responses[2].data.data[i].grade;
                        }
                        studentAvg = studentAvg === 0 ? 0 : studentAvg / responses[2].data.data.length;
                        const projectAverage =
                            (studentAvg + instGrade) /
                            (responses[0].data.data.length + responses[1].data.data.length + 1);
                        const grades = {
                            persons: persons,
                            studentAvg: studentAvg,
                            projectAverage: projectAverage.toFixed(2),
                            courseAverage: studentAvg.toFixed(2),
                            finalGrade: 40, //kendim belirledim
                        };
                        this.setState({ feedbacks: feedbacks, grades: grades });
                    })
                );
            });
        });
        const feedbackRequests = [];
        const persons = [];

        /*const dummyFeedbacks = {
    SRSResult: {
        grade: '9.5',
        maxGrade: '11',
    },*/
    }

    onReturnProjectPage = () => {
        this.props.history.replace('/project/' + this.props.projectId);
    };

    onSubmissionModalOpened = (modalType) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: '',
            });
        }
    };

    onExistingSubmissionModalOpened = (modalType, submissionCaption, submissionFile) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: submissionCaption,
                submissionFile: submissionFile,
            });
        }
    };

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

    onSubmissionCaptionChanged = (e) => {
        e.preventDefault();
        this.setState({
            submissionCaption: e.target.value,
        });
    };

    // modals
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

    onModalOpenedWithCommentOpened = (
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

    getSubmissionButtons = () => {
        if (this.state.submissionPage?.isInGroup) {
            if (!this.state.submissionPage.hasSubmission) {
                return (
                    <Button onClick={() => this.onSubmissionModalOpened('isAddSubmissionOpen')}>
                        Add New Submission
                    </Button>
                );
            } else if (this.state.submissionPage.isEdittable) {
                return (
                    <>
                        <Button
                            onClick={() =>
                                this.onExistingSubmissionModalOpened(
                                    'isEditSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.hasFile
                                )
                            }>
                            Edit Submission
                        </Button>
                        <Button
                            onClick={() =>
                                this.onExistingSubmissionModalOpened(
                                    'isDeleteSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.hasFile
                                )
                            }>
                            Delete Submission
                        </Button>
                    </>
                );
            }
        }

        return null;
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
                        this.props.projectName
                    )}
                    finalGrade={this.state.grades?.finalGrade}
                />
            ) : (
                <div>No Grades</div>
            ),
        };
    };

    getNewCommentButton = () => {
        let newCommentButton = null;
        if (this.state.submissionPage?.canUserComment && !this.state.isInGroup) {
            newCommentButton = (
                <Button
                    content="Give Feedback"
                    labelPosition="right"
                    icon="edit"
                    primary
                    onClick={() => this.onModalOpened('isGiveFeedbackOpen', false)}
                />
            );
        }

        return newCommentButton;
    };

    onAuthorClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    getFeedbacksPane = () => {
        const newCommentButton = this.getNewCommentButton();
        const content = (
            <Grid>
                <div class="sixteen wide column">
                    <NewCommentModal2
                        text={this.state.currentFeedbackText2}
                        grade={this.state.currentFeedbackGrade2}
                        maxGrade={this.state.currentMaxFeedbackGrade2}
                        onTextChange={(e) => this.onCurrentFeedbackTextChanged2(e)}
                        onGradeChange={(e) => this.onCurrentFeedbackGradeChanged2(e)}
                        onMaxGradeChange={(e) => this.onCurrentFeedbackMaxGradeChanged2(e)}
                        onGiveFeedback={(e) => this.onGiveFeedback(e)}
                    />
                </div>
                <div class="sixteen wide column" style={{ marginTop: '-20px' }}>
                    <Divider />
                </div>
                <div class="sixteen wide column" style={{ marginTop: '-20px' }}>
                    <FeedbacksPane
                        feedbacksAccordion={getFeedbacksAsAccordion(
                            this.state.feedbacks,
                            this.state.submissionPage?.isTAorInstructor,
                            this.onModalOpenedWithCommentOpened,
                            this.onAuthorClicked,
                            this.props.userId,
                            this.onModalOpened
                        )}
                        //newCommentButton={newCommentButton}
                    />
                </div>
            </Grid>
        );
        return {
            title: 'Feedbacks',
            content: content,
        };
    };

    getPaneElements = () => {
        const assignment = {
            ...this.state.assignment,
            date:
                (typeof this.state.assignment.publishmentDate === 'object'
                    ? dateObjectToString(this.state.assignment.publishmentDate)
                    : this.state.assignment.publishmentDate) +
                ' / ' +
                (typeof this.state.assignment.dueDate === 'object'
                    ? dateObjectToString(this.state.assignment.dueDate)
                    : this.state.assignment.dueDate),
        };

        const submissionButtons = this.getSubmissionButtons();

        let submissionPane;
        if (this.state.submissionPage?.hasSubmission) {
            if (
                this.state.submissionPage.isSubmissionAnonim &&
                !this.state.submissionPage.isTAorInstructor &&
                !this.state.submissionPage.isInGroup
            ) {
                submissionPane = (
                    <SubmissionPane
                        assignment={assignment}
                        submission={'anonim'}
                        isLate={this.state.submissionPage?.isLate}
                        onAssignmentFileClicked={this.onAssignmentFileClicked}
                        buttons={submissionButtons}
                    />
                );
            } else {
                submissionPane = (
                    <SubmissionPane
                        assignment={assignment}
                        submission={this.state.submission}
                        isLate={this.state.submissionPage?.isLate}
                        onAssignmentFileClicked={this.onAssignmentFileClicked}
                        onSubmissionFileClicked={this.onSubmissionFileClicked}
                        buttons={submissionButtons}
                    />
                );
            }
        } else {
            submissionPane = (
                <SubmissionPane
                    assignment={assignment}
                    onAssignmentFileClicked={this.onAssignmentFileClicked}
                    buttons={submissionButtons}
                />
            );
        }

        return [
            {
                title: 'Submission',
                content: this.state.submissionPage ? submissionPane : <div>No Such Assignment</div>,
            },
            this.getGradesPane(),
            this.getFeedbacksPane(),
        ];
    };

    // Modals
    getModals = () => {
        return (
            <>
                <NewSubmissionModal
                    isOpen={this.state.isAddSubmissionOpen}
                    instructions={this.state.assignment.submissionInfo}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isAddSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChanged(e)}
                    onFileChanged={this.onSubmissionFileChange}
                />
                <EditSubmissionModal
                    isOpen={this.state.isEditSubmissionOpen}
                    instructions={this.state.assignment.submissionInfo}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isEditSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChanged(e)}
                />
                <DeleteSubmissionModal
                    isOpen={this.state.isDeleteSubmissionOpen}
                    closeModal={(isSuccess) => this.onSubmissionModalClosed('isDeleteSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                />
                <NewCommentModal
                    isOpen={this.state.isGiveFeedbackOpen}
                    closeModal={(isSuccess) => this.onModalClosed('isGiveFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
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
                    projectName={this.props.projectName}
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
                    projectName={this.props.projectName}
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
                    Back To Assigments
                </Link>
                <Tab tabPanes={this.getPaneElements()} />
                {this.getModals()}
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return { userId: state.userId, token: state.token };
};

export default withRouter(connect(mapStateToProps)(ProjectAssignment));

const dummyAssignment = {
    title: 'CS319-2021Spring / Desing Report Assignment',
    status: 'graded',
    caption:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis numquam voluptas deserunt a nemo architecto assumenda suscipit ad! Doloribus dolorum ducimus laudantium exercitationem fugiat. Quibusdam ad soluta animi quasi! Voluptatum.',
    publisher: 'Erdem Tuna',
    publishmentDate: new Date(2023, 3, 13, 12, 0),
    dueDate: new Date(2025, 4, 16, 23, 59),
    file: 'deneme',
    submissionInfo: 'Please name your submission file as: surname_name_id_section.pdf',
};

const dummySubmission = {
    caption:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Provident dicta dignissimos dolore quo iure, et ipsam corporis accusamus ad eligendi, inventore consequatur, repellendus laboriosam vitae sed quam fugit. Omnis dignissimos eos libero facilis quisquam quidem. Labore veritatis eaque non vero asperiores, soluta, qui nisi adipisci, fugit corrupti praesentium voluptatem enim?',
    file: 'file',
    date: new Date(2025, 4, 16, 12, 31),
    submissionId: 1,
};

const dummyFeedbacks = {
    SRSResult: {
        grade: '9.5',
        maxGrade: '11',
    },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 3, 11, 12, 0),
            commentId: 3,
            userId: 'dD3wUcJiDHTM9aDs8livI9HpY3h2',
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: new Date(2021, 3, 11, 12, 0),
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            //name: 'Eray Tüzün',
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            //grade: '9.5',
            date: new Date(2021, 3, 11, 12, 0),
            commentId: 5,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
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
            date: new Date(2021, 3, 11, 12, 0),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
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
            date: new Date(2021, 3, 11, 12, 0),
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: new Date(2021, 3, 11, 12, 0),
            caption:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
