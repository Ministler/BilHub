import React, { Component } from 'react';
import { TextArea, Icon, Grid } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Profile.css';
import { TabExampleSecondaryPointing, ProfilePrompt } from './ProfileComponents';

class Profile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userInformation: null,
            projects: null,
            courses: null,

            informationEditMode: false,
            newInformation: '',
        };
        console.log(this.props);
    }

    changeUserInformation = (newInformation) => {
        const request = {
            newInformation: newInformation,
            userId: this.props.userId,
        };

        console.log(request);
    };

    componentDidMount() {
        this.setState({
            userInformation: dummyUser.userInformation,
            projects: dummyProjects,
            courses: dummyInstructedCourses,

            newInformation: dummyUser.userInformation,
        });
    }

    onNewInformationChanged = (e) => {
        e.preventDefault();
        this.setState({
            newInformation: e.target.value,
        });
    };

    onInformationEditModeToggled = () => {
        this.setState((prevState) => {
            return {
                informationEditMode: !prevState['informationEditMode'],
            };
        });
    };

    onUserClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    onProjectClicked = (projectId) => {
        this.props.history.push('/project/' + projectId);
    };

    onCourseClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    getInformationElement = () => {
        let groupInformationElement = <p>{this.state.userInformation}</p>;
        if (this.state.informationEditMode) {
            groupInformationElement = (
                <TextArea onChange={(e) => this.onNewInformationChanged(e)} value={this.state.newInformation} />
            );
        }
        return groupInformationElement;
    };

    getEditInformationIcon = () => {
        let informationEditIcon = null;
        if (!this.props.match.params.id || this.props.match.params.id === this.props.userId) {
            informationEditIcon = this.state.informationEditMode ? (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.changeUserInformation(this.state.newInformation);
                        this.onInformationEditModeToggled();
                    }}
                    name={'check'}
                    color="blue"
                    style={{float: "right", marginTop: "-10px"}}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.onInformationEditModeToggled();
                    }}
                    name={'edit'}
                    color="blue"
                    style={{float: "right", marginTop: "-15px"}}
                />
            );
        }
        return informationEditIcon;
    };

    render() {
        return (
            <div class="ui centered grid">
                <Grid.Row divided>
                    <div class="four wide column">
                        <ProfilePrompt
                            name={this.props.name}
                            email={this.props.email}
                            informationElement={this.getInformationElement()}
                            icon={this.getEditInformationIcon()}
                        />
                    </div>
                    <div class="twelve wide column">
                        <TabExampleSecondaryPointing
                            userType={this.props.userType}
                            projects={this.state.projects}
                            courses={this.state.courses}
                            onUserClicked={(userId) => this.onUserClicked(userId)}
                            onProjectClicked={(projectId) => this.onProjectClicked(projectId)}
                            onCourseClicked={(courseId) => this.onCourseClicked(courseId)}
                        />
                    </div>
                </Grid.Row>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        userType: state.userType,
        userId: state.userId,
        email: state.email,
        name: state.name,
    };
};

export default connect(mapStateToProps)(Profile);

const dummyUser = {
    userInformation:
        'Lorem ipsum dolor sit amet consectetur adipisicing elit. Delectus id aspernatur ea sitanimi, ab qui! Ea beatae dolorum inventore cum quibusdam placeat quisquam itaque, odioquasi numquam maiores quidem illum odit commodi dicta animi voluptas tempora? Adipisci maiores inventore minus provident quas minima itaque saepe et labore, ut sequi!',
};

const dummyProjects = [
    {
        courseName: 'CS319-2021Spring',
        groupName: 'BilHub',
        projectId: 1,
        isActive: true,
        groupMembers: [
            { name: 'Barış Ogün Yörük', information: 'Frontend', userId: 1 },
            { name: 'Halil Özgür Demir', information: 'Frontend', userId: 2 },
            { name: 'Yusuf Uyar Miraç', information: 'Frontend', userId: 3 },
            { name: 'Aybala Karakaya', information: 'Backend', userId: 4 },
            { name: 'Çağrı Mustafa Durgut', information: 'Backend', userId: 5 },
            { name: 'Oğuzhan Özçelik', information: 'Database', userId: 6 },
        ],
        peerGrade: '10/10',
        projectGrade: '10/10',
    },
    {
        courseName: 'CS319-2021Spring',
        groupName: 'BilHub',
        isActive: true,
        projectId: 1,
        groupMembers: [
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
        ],
        peerGrade: '10/10',
        projectGrade: '10/10',
    },
    {
        courseName: 'CS319-2021Spring',
        groupName: 'BilHub',
        isActive: false,
        projectId: 1,
        groupMembers: [
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
        ],
        peerGrade: '10/10',
        projectGrade: '10/10',
    },
];

const dummyInstructedCourses = [
    {
        courseName: 'CS484-2020Fall',
        courseId: 1,
        TAs: [
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 2,
            },
        ],
        instructors: [
            {
                name: 'Eray Tüzün',
                userId: 3,
            },
            {
                name: 'Eray Tüzün',
                userId: 4,
            },
        ],
    },
    {
        courseName: 'CS484-2020Fall',
        courseId: 2,
        TAs: [
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
            {
                name: 'Aybala Karakaya',
                userId: 1,
            },
        ],
        instructors: [
            {
                name: 'Eray Tüzün',
                userId: 1,
            },
            {
                name: 'Eray Tüzün',
                userId: 1,
            },
        ],
    },
];
