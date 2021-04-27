import React, { Component } from 'react';
import { Icon, Dropdown, Button } from 'semantic-ui-react';
import { withRouter } from 'react-router-dom';

import './CourseAssignment.css';
import { AssignmentCardElement, Tab, getSubmissionsAsAccordion } from '../../../components';

class CourseAssignment extends Component {
    constructor(props) {
        super(props);
        this.state = {
            assignment: null,
            submissions: null,
            currentSection: 0,
        };
    }

    componentDidMount() {
        this.setState({
            assignment: dummyAssignment,
            submissions: dummyGroupSubmissions,
        });
    }

    onReturnProjectPage = () => {
        this.props.history.replace('/course/' + this.props.match.params.courseId);
    };

    onFileClicked = () => {
        console.log('File');
    };

    getAssignmentControlIcons = () => {
        let controlIcons = null;
        if (this.props.isCourseActive && this.state.assignment?.isUserTAorInstructor) {
            controlIcons = (
                <>
                    <Icon name="edit" />
                    <Icon name="close" />
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
                    fileIcon={this.state.assignment?.file ? <Icon name="file" /> : null}
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
                    defaultValue={0}
                    fluid
                    selection
                    options={sectionOptions}
                    onChange={(e, dropdownValues) => this.onSectionChanged(dropdownValues)}
                />
            );
        }

        return null;
    };

    onSubmissionPageClicked = (projectId, submissionPageId) => {
        this.props.history.push('/project/' + projectId + '/submission/' + submissionPageId);
    };

    onSubmissionFileClicked = () => {
        console.log('CLICKED');
    };

    getSubmissionsPane = () => {
        return {
            title: 'Submissions',
            content: (
                <>
                    {this.getDropdownForSection()}
                    {this.state.submissions?.length >= 1
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
            content: <div>Denemes</div>,
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
                    <Button icon={'download'}>Download All Files</Button>
                    <Button icon={'download'}>Donwload Only Submitted Files</Button>
                </>
            );
        }
        return buttons;
    };

<<<<<<< HEAD
    render() {
        return (
            <div>
                <Icon onClick={this.onReturnProjectPage} size="huge" name="angle left" />
                <Tab tabPanes={this.getPaneElements()} />
=======
function RequestJoinMerge() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Request Group Join & Merge</Button>}
            size={'mini'}>
            <Modal.Header style={{ fontSize: '16px' }}>Request Group Join & Merge</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid centered>
                        <div className="ui warning message" style={{ fontSize: '12px', width: '95%' }}>
                            Please write a note and send a request
                        </div>
                        <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => setOpen(false)}
                    style={{
                        borderRadius: '10px',
                        padding: '5px 16px',
                        fontSize: '14px',
                        fontWeight: '500',
                        lineHeight: '20px',
                        whiteSpace: 'nowrap',
                    }}>
                    Send
                </Button>
            </Modal.Actions>
        </Modal>
    );
}

function RequestJoin() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Request Group Join</Button>}
            size={'mini'}>
            <Modal.Header style={{ fontSize: '16px' }}>Request Group Join</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid centered>
                        <div className="ui warning message" style={{ fontSize: '12px', width: '95%' }}>
                            Please write a note and send a request
                        </div>
                        <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => setOpen(false)}
                    style={{
                        borderRadius: '10px',
                        padding: '5px 16px',
                        fontSize: '14px',
                        fontWeight: '500',
                        lineHeight: '20px',
                        whiteSpace: 'nowrap',
                    }}>
                    Send
                </Button>
            </Modal.Actions>
        </Modal>
    );
}

function GroupReady() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Ready Status</Button>}
            style={{ width: '38%' }}>
            <Modal.Header style={{ fontSize: '16px' }}>Ready Status</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Form>
                        <Form.Field>
                            <Checkbox label="Ready 3/5" />
                            <Link
                                style={{
                                    fontSize: '14px',
                                    float: 'right',
                                }}
                                to="/notifications">
                                Chek join requests
                            </Link>
                        </Form.Field>
                        <Button
                            floated="right"
                            negative
                            onClick={() => setOpen(false)}
                            style={{
                                borderRadius: '10px',
                                padding: '5px 16px',
                                fontSize: '14px',
                                fontWeight: '500',
                                lineHeight: '20px',
                                whiteSpace: 'nowrap',
                                marginBottom: '10px',
                            }}>
                            Exit Group
                        </Button>
                    </Form>
                </Modal.Description>
            </Modal.Content>
        </Modal>
    );
}

const options = [
    { key: 1, text: 'Submission', value: 1 },
    { key: 2, text: 'Peer Review', value: 2 },
];

function AsignmentSetting() {
    const [open, setOpen] = React.useState(false);

    return (
        <Modal
            closeIcon
            onClose={() => setOpen(false)}
            onOpen={() => setOpen(true)}
            open={open}
            trigger={<Button>Asignment Setting</Button>}
            size={'small'}>
            <Modal.Header style={{ fontSize: '16px' }}>Asignment Setting</Modal.Header>
            <Modal.Content>
                <Modal.Description>
                    <Grid>
                        <Form>
                            <Form.Group inline>
                                <p style={{ fontSize: '14px', float: 'left', marginBottom: '0px', display: 'inline' }}>
                                    Title
                                </p>
                                <Dropdown
                                    item
                                    simple
                                    text="Assignment Type"
                                    direction="right"
                                    options={options}
                                    style={{ marginLeft: '510px', float: 'right', display: 'inline' }}
                                />
                            </Form.Group>
                            <Form.Input
                                type="text"
                                name="groupName"
                                style={{ width: '40%', height: '35px', marginTop: '-15px' }}
                            />
                            <label style={{ fontSize: '14px', float: 'left', marginBottom: '5px' }}>Description</label>
                            <TextArea placeholder="Your message here" style={{ minHeight: 100, width: '95%' }} />
                            <Form.Group grouped>
                                <Form.Field label="Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Anonymous Student Comments" control="input" type="checkbox" />
                                <Form.Field label="Graded Comments" control="input" type="checkbox" />
                                <Form.Field label="Groups' Submission Visable" control="input" type="checkbox" />
                                <Form.Field label="Late Submission" control="input" type="checkbox" />
                                <Form.Field label="Due Date" />
                                <Form.Field label="Add File" />
                            </Form.Group>
                        </Form>
                    </Grid>
                </Modal.Description>
            </Modal.Content>
            <Modal.Actions>
                <Button
                    color="blue"
                    onClick={() => setOpen(false)}
                    style={{
                        borderRadius: '10px',
                        padding: '5px 16px',
                        fontSize: '14px',
                        fontWeight: '500',
                        lineHeight: '20px',
                        whiteSpace: 'nowrap',
                    }}>
                    Send
                </Button>
            </Modal.Actions>
        </Modal>
    );
}

export class CourseAssignment extends Component {
    render() {
        return (
            <div class="ui centered grid">
                <RequestJoinMerge></RequestJoinMerge>
                <RequestJoin></RequestJoin>
                <GroupReady></GroupReady>
                <AsignmentSetting></AsignmentSetting>
>>>>>>> yusuf-dev
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
    numberOfSections: 5,
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
                submissionPageId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionPageId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionPageId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionPageId: 2,
            },
        ],
        notSubmitted: [
            {
                groupName: 'BilHub',
                projectId: 1,
                submissionPageId: 1,
            },
            {
                groupName: 'Classroom Helper',
                projectId: 2,
                submissionPageId: 2,
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
                submissionPageId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                grade: '7/10',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionPageId: 2,
            },
        ],
        submitted: [
            {
                groupName: 'BilHub',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 1,
                submissionPageId: 1,
            },
            {
                groupName: 'Classroom Helper',
                fileName: '1_1_analysisReport.pdf',
                file: 'file',
                submissionDate: '15 March 2021',
                projectId: 2,
                submissionPageId: 2,
            },
        ],
        notSubmitted: [
            {
                groupName: 'BilHub',
                projectId: 1,
                submissionPageId: 1,
            },
            {
                groupName: 'Classroom Helper',
                projectId: 2,
                submissionPageId: 2,
            },
        ],
    },
];

export default withRouter(CourseAssignment);
