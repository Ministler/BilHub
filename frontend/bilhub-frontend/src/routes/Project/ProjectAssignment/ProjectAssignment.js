import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Icon, Button, Card } from 'semantic-ui-react';
import { withRouter } from 'react-router-dom';

import {
    GradePane,
    FeedbackPane,
    NewCommentModal,
    EditCommentModal,
    DeleteCommentModal,
    SubmissionPane,
    NewSubmissionModal,
    EditSubmissionModal,
    DeleteSubmissionModal,
} from '../ProjectComponents';
import { Tab, FeedbackCardElement } from '../../../commonComponents';

class ProjectAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            assignment: {},
            submission: {},
            grades: null,
            feedbacks: [],

            // State models regarding submission
            isAddSubmissionOpen: false,
            isEditSubmissionOpen: false,
            isDeleteSubmissionOpen: false,
            submissionFile: null,
            submissionCaption: '',

            // States models regarding feedbacks
            isGiveFeedbackOpen: false,
            isEditFeedbackOpen: false,
            isDeleteFeedbackOpen: false,
            isFeedbackSRS: false,
            currentFeedbackText: '',
            currentFeedbackFile: null,
            currentFeedbackGrade: 10,
            currentFeedbackId: 0,

            // States regarding accordion
            accordionActiveIndex: 0,
        };
    }

    componentDidMount() {
        this.setState({
            assignment: dummyAssignment,
            grades: dummyGrades,
            feedbacks: dummyFeedbacks,
            submission: dummySubmission,
        });
    }

    returnProjectPage = () => {
        this.props.history.replace('/project/' + this.props.projectId);
    };

    getSRSFeedbackContent = (SRSResult) => {
        if (SRSResult) {
            let icons = null;
            if (this.state.assignment.isTAorInstructor) {
                icons = (
                    <span>
                        <Icon
                            name="edit"
                            onClick={() =>
                                this.openModelWithComment(
                                    'isEditFeedbackOpen',
                                    true,
                                    SRSResult.commentId,
                                    SRSResult.feedback,
                                    SRSResult.grade
                                )
                            }
                        />
                        <Icon
                            name="delete"
                            onClick={() =>
                                this.openModelWithComment(
                                    'isDeleteFeedbackOpen',
                                    true,
                                    SRSResult.commentId,
                                    SRSResult.feedback,
                                    SRSResult.grade
                                )
                            }
                        />
                    </span>
                );
            }

            return (
                <FeedbackCardElement
                    feedback={SRSResult.feedback}
                    grade={SRSResult.grade}
                    totalGrade={10}
                    author={SRSResult.name}
                    date={SRSResult.date}
                    icons={icons}
                />
            );
        } else if (this.state.assignment?.isTAorInstructor) {
            return <Button onClick={() => this.openModal('isGiveFeedbackOpen', true)}>Add SRS Grade</Button>;
        } else {
            return <div>No Feedback</div>;
        }
    };

    getFeedbacksContent = (feedbacks) => {
        if (feedbacks) {
            const feedbackFeedElements = feedbacks.map((feedback) => {
                let icons = null;
                if (this.state.userId === feedback.userId) {
                    icons = (
                        <span>
                            <Icon
                                name="edit"
                                onClick={() =>
                                    this.openModelWithComment(
                                        'isEditFeedbackOpen',
                                        false,
                                        feedback.commentId,
                                        feedback.feedback,
                                        feedback.grade
                                    )
                                }
                            />
                            <Icon
                                name="delete"
                                onClick={() =>
                                    this.openModelWithComment(
                                        'isDeleteFeedbackOpen',
                                        false,
                                        feedback.commentId,
                                        feedback.feedback,
                                        feedback.grade
                                    )
                                }
                            />
                        </span>
                    );
                }

                return (
                    <FeedbackCardElement
                        feedback={feedback.feedback}
                        grade={feedback.grade}
                        totalGrade={10}
                        author={feedback.name}
                        date={feedback.date}
                        icons={icons}
                    />
                );
            });

            return <Card.Group>{feedbackFeedElements}</Card.Group>;
        } else {
            return <div>No Feedback</div>;
        }
    };

    // Accordion
    handleAccordionClick = (titleProps) => {
        const index = titleProps.index;
        const activeIndex = this.state.accordionActiveIndex;
        const newIndex = activeIndex === index ? -1 : index;

        this.setState({ accordionActiveIndex: newIndex });
    };

    onAssignmentFileClicked = () => {
        console.log('file');
    };

    onSubmissionFileClicked = () => {
        console.log('file');
    };

    openSubmissionModel = (modalType) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: '',
                submissionFile: 'empty',
            });
        }
    };

    openExistingSubmissionModel = (modalType, submissionCaption, submissionFile) => {
        if (modalType) {
            this.setState({
                [modalType]: true,
                submissionCaption: submissionCaption,
                submissionFile: submissionFile,
            });
        }
    };

    closeSubmissionModal = (modalType, isSuccess) => {
        this.setState({
            [modalType]: false,
        });
        if (!isSuccess) return;
    };

    getSubmissionButtons = () => {
        if (this.state.submission?.isInGroup) {
            if (this.state.submission.noSubmissionYet) {
                return (
                    <Button onClick={() => this.openSubmissionModel('isAddSubmissionOpen')}>Add New Submission</Button>
                );
            } else {
                return (
                    <>
                        <Button
                            onClick={() =>
                                this.openExistingSubmissionModel(
                                    'isEditSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.file
                                )
                            }>
                            Edit Submission
                        </Button>
                        <Button
                            onClick={() =>
                                this.openExistingSubmissionModel(
                                    'isDeleteSubmissionOpen',
                                    this.state.submission.caption,
                                    this.state.submission.file
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

    getPaneElements = () => {
        let newCommentButton = null;
        if (this.state.assignment?.canUserComment) {
            newCommentButton = (
                <Button
                    content="Give Feedback"
                    labelPosition="right"
                    icon="edit"
                    primary
                    onClick={() => this.openModal('isGiveFeedbackOpen', false)}
                />
            );
        }

        const assignment = {
            ...this.state.assignment,
            date: this.state.assignment.publishmentDate + ' / ' + this.state.assignment.dueDate,
        };

        const submissionButtons = this.getSubmissionButtons();
        let submissionPane = (
            <SubmissionPane
                assignment={assignment}
                submission={this.state.submission}
                onAssignmentFileClicked={this.onAssignmentFileClicked}
                onSubmissionFileClicked={this.onSubmissionFileClicked}
                buttons={submissionButtons}
            />
        );

        if (this.state.submission?.noSubmissionYet) {
            submissionPane = (
                <SubmissionPane
                    assignment={assignment}
                    submission={null}
                    onAssignmentFileClicked={this.onAssignmentFileClicked}
                    buttons={submissionButtons}
                />
            );
        }

        return [
            {
                title: 'Submission',
                content:
                    this.state.assignment && this.state.submission ? submissionPane : <div>No Such Assignment</div>,
            },
            {
                title: 'Grades',
                content: this.state.grades ? (
                    <GradePane
                        firstBodyRowsData={this.getTableBodyRowsData(this.state.grades)}
                        firstHeaderNames={['User', 'Person', 'Grade']}
                        secondBodyRowsData={[[this.state.grades.projectAverage, this.state.grades.courseAverage]]}
                        secondHeaderNames={[this.props.projectName + ' Avarage', 'Course Average']}
                        finalGrade={this.state.grades.finalGrade}
                    />
                ) : (
                    <div>No Grades</div>
                ),
            },
            {
                title: 'Feedbacks',
                content: (
                    <FeedbackPane
                        handleClick={(titleProps) => this.handleAccordionClick(titleProps)}
                        activeIndex={this.state.accordionActiveIndex}
                        newCommentButton={newCommentButton}
                        accordionElements={this.getCommentsAsAccordionElements(
                            this.getSRSFeedbackContent(this.state.feedbacks.SRSResult),
                            this.getFeedbacksContent(this.state.feedbacks.InstructorComments),
                            this.getFeedbacksContent(this.state.feedbacks.TAComments),
                            this.getFeedbacksContent(this.state.feedbacks.StudentComments)
                        )}></FeedbackPane>
                ),
            },
        ];
    };

    onCurrentFeedbackTextChange = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackText: e.target.value,
        });
    };

    onCurrentFeedbackGradeChange = (e) => {
        e.preventDefault();
        this.setState({
            currentFeedbackGrade: e.target.value,
        });
    };

    getTableBodyRowsData = (grades) => {
        const persons = grades.persons;

        const personsData = persons.map((person) => {
            return [person.type, person.name, person.grade];
        });

        if (grades.studentsAverage) {
            personsData.push(['Students Average', '', grades.studentsAverage]);
        }

        return personsData;
    };

    getCommentsAsAccordionElements = () => {
        const feedbacks = this.state.feedbacks;

        return [
            { title: 'SRS Feedback', content: this.getSRSFeedbackContent(feedbacks.SRSResult) },
            {
                title: 'Instructor Feedbacks',
                content: this.getFeedbacksContent(feedbacks.InstructorComments),
            },
            { title: 'TA Feedbacks', content: this.getFeedbacksContent(feedbacks.TAComments) },
            { title: 'Student Feedbacks', content: this.getFeedbacksContent(feedbacks.StudentComments) },
        ];
    };

    // Models
    openModal = (modelType, isFeedbackSRS) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modelType) {
            this.setState({
                [modelType]: true,
                currentFeedbackGrade: 10,
                currentFeedbackFile: 'empty',
                currentFeedbackText: '',
            });
        }
    };

    openModelWithComment = (modelType, isFeedbackSRS, commentId, commentText, commentGrade, commentFile) => {
        if (isFeedbackSRS) {
            this.setState({
                isFeedbackSRS: true,
            });
        } else {
            this.setState({
                isFeedbackSRS: false,
            });
        }

        if (modelType) {
            this.setState({
                currentFeedbackId: commentId,
                [modelType]: true,
                currentFeedbackText: commentText,
                currentFeedbackGrade: commentGrade,
                currentFeedbackFile: commentFile,
            });
        }
    };

    closeModal = (modelType, isSuccess) => {
        this.setState({
            [modelType]: false,
        });
        if (!isSuccess) return;

        if (this.state.isFeedbackSRS) {
            let feed = { ...this.state.feedbacks.SRSResult };
            if (this.state.isEditFeedbackOpen) {
                feed.feedback = this.state.currentFeedbackText;
                feed.grade = this.state.currentFeedbackGrade;
            } else if (this.state.isGiveFeedbackOpen) {
                feed.feedback = this.state.currentFeedbackText;
                feed.grade = this.state.currentFeedbackGrade;
            } else if (this.state.isDeleteFeedbackOpen) {
                feed = null;
            }
            this.setState({
                feedbacks: {
                    ...this.state.feedbacks,
                    SRSResult: feed,
                },
            });
            return;
        }

        if (this.state.isEditFeedbackOpen) {
        } else if (this.state.isGiveFeedbackOpen) {
        } else if (this.state.isDeleteFeedbackOpen) {
        }

        // POST
    };

    onSubmissionCaptionChange = (e) => {
        e.preventDefault();
        this.setState({
            submissionCaption: e.target.value,
        });
    };

    // Modals
    getModals = () => {
        return (
            <>
                <NewSubmissionModal
                    isOpen={this.state.isAddSubmissionOpen}
                    closeModal={(isSuccess) => this.closeModal('isAddSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChange(e)}
                />
                <EditSubmissionModal
                    isOpen={this.state.isEditSubmissionOpen}
                    closeModal={(isSuccess) => this.closeModal('isEditSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                    onTextChange={(e) => this.onSubmissionCaptionChange(e)}
                />
                <DeleteSubmissionModal
                    isOpen={this.state.isDeleteSubmissionOpen}
                    closeModal={(isSuccess) => this.closeModal('isDeleteSubmissionOpen', isSuccess)}
                    assignmentName={this.state.assignment.title}
                    text={this.state.submissionCaption}
                />
                <NewCommentModal
                    isOpen={this.state.isGiveFeedbackOpen}
                    closeModal={(isSuccess) => this.closeModal('isGiveFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChange(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChange(e)}
                />
                <EditCommentModal
                    isOpen={this.state.isEditFeedbackOpen}
                    closeModal={(isSuccess) => this.closeModal('isEditFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                    onTextChange={(e) => this.onCurrentFeedbackTextChange(e)}
                    onGradeChange={(e) => this.onCurrentFeedbackGradeChange(e)}
                />
                <DeleteCommentModal
                    isOpen={this.state.isDeleteFeedbackOpen}
                    closeModal={(isSuccess) => this.closeModal('isDeleteFeedbackOpen', isSuccess)}
                    projectName={this.props.projectName}
                    isTitleSRS={this.state.isFeedbackSRS}
                    text={this.state.currentFeedbackText}
                    grade={this.state.currentFeedbackGrade}
                />
            </>
        );
    };

    render() {
        const paneElements = this.getPaneElements();
        const modals = this.getModals();
        return (
            <div>
                <Icon onClick={this.returnProjectPage} size="huge" name="angle left" />
                <Tab panes={paneElements} />
                {modals}
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
    publisherId: 1,
    publishmentDate: '13 March 2023 12:00',
    dueDate: '16 April 2025, 23:59',
    file: 'deneme',
    submissionInfo: 'Please name your submission file as: surname_name_id_section.pdf',
};

const dummySubmission2 = {
    isInGroup: true,
    noSubmissionYet: true,
    isTAorInstructor: true,
};

const dummySubmission = {
    isAnonoym: true,
    isTAorInstructor: true,
    canUserComment: true,
    caption:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Provident dicta dignissimos dolore quo iure, et ipsam corporis accusamus ad eligendi, inventore consequatur, repellendus laboriosam vitae sed quam fugit. Omnis dignissimos eos libero facilis quisquam quidem. Labore veritatis eaque non vero asperiores, soluta, qui nisi adipisci, fugit corrupti praesentium voluptatem enim?',
    isInGroup: true,
    file: 'file',
    date: '16 April 2025, 12:31',
    projectId: 1,
    submissionId: 1,
};

const dummyGrades = {
    persons: [
        {
            name: 'Eray Tüzün',
            type: 'Project Instructor',
            grade: '9.5',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            type: 'Instructor',
            grade: '8.1',
            userId: 2,
        },
        {
            name: 'Erdem Tuna',
            type: 'TA',
            grade: '7.1',
            userId: 3,
        },
        {
            name: 'Elgun Jabrayilzade',
            type: 'TA',
            grade: '8.1',
            userId: 4,
        },
    ],
    studentsAverage: 8,
    projectAverage: 7.1,
    courseAverage: 6.5,
    finalGrade: 38,
};

const dummyFeedbacks = {
    SRSResult: {
        name: 'Elgun Jabrayilzade',
        feedback: 'Please download the complete feedback file',
        file: 'dummyFile',
        date: '11 March 2021',
        commentId: 1,
        grade: 9.5,
    },
    InstructorComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 3,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    TAComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 4,
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
    StudentComments: [
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            commentId: 5,
            userId: 1,
            userGroupName: 'ClassRoom Helper',
            userGroupId: 5,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
            userGroupName: 'ProjectManager',
            userGroupId: 4,
        },
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
        {
            name: 'Eray Tüzün',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            grade: '9.5',
            date: '11 March 2021',
            userId: 1,
        },
        {
            name: 'Alper Sarıkan',
            date: '11 March 2021',
            feedback:
                'Lorem ipsum dolor sit, amet consectetur adipisicing elit. Cumque neque ullam a ad quia aut vitae voluptate animi dolor delectus?',
            file: 'dummyFile',
            grade: '8.1',
            userId: 2,
        },
    ],
};
