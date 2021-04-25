import React, { Component } from 'react';
import { TextArea, Icon } from 'semantic-ui-react';
import { connect } from 'react-redux';

import './Profile.css';
import { TabExampleSecondaryPointing, ProfilePrompt } from './ProfileComponents';

class Profile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user: null,
            projects: null,
            instructedCourse: null,

            informationEditMode: false,
            newInformation: '',
        };
    }

    componentDidMount() {
        this.setState({
            user: dummyUser,
            projects: dummyProjects,
            courses: dummyInstructedCourses,
            newInformation: dummyUser.information,
        });
    }

    onNewInformationChange = (e) => {
        e.preventDefault();
        this.setState({
            newInformation: e.target.value,
        });
    };

    changeUserInformation = (newInformation) => {
        const user = { ...this.state.user, information: newInformation };
        this.setState({
            user: user,
        });
    };

    toggleInformationEditMode = () => {
        this.setState((prevState) => {
            return {
                informationEditMode: !prevState['informationEditMode'],
            };
        });
    };

    getInformationElement = () => {
        let groupInformationElement = <p>{this.state.user?.information}</p>;
        if (this.state.informationEditMode) {
            groupInformationElement = (
                <TextArea onChange={(e) => this.onNewInformationChange(e)} value={this.state.newInformation} />
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
                        this.toggleInformationEditMode();
                    }}
                    name={'check'}
                />
            ) : (
                <Icon
                    className="clickableChangeColor"
                    onClick={() => {
                        this.toggleInformationEditMode();
                    }}
                    name={'edit'}
                />
            );
        }
        return informationEditIcon;
    };

    onMemberClicked = (userId) => {
        this.props.history.push('/profile/' + userId);
    };

    onProjectTitleClicked = (projectId) => {
        this.props.history.push('/project/' + projectId);
    };

    onCourseTitleClicked = (courseId) => {
        this.props.history.push('/course/' + courseId);
    };

    render() {
        return (
            <div class="ui centered grid">
                <div class="row">
                    <div class="four wide column">
                        <ProfilePrompt
                            name={this.props.name}
                            email={this.props.email}
                            informationElement={this.getInformationElement()}
                            icon={this.getEditInformationIcon()}
                        />
                    </div>
                    <div class="eight wide column">
                        <TabExampleSecondaryPointing
                            userType={this.props.userType}
                            projects={this.state.projects}
                            courses={this.state.courses}
                            onMemberClicked={(userId) => this.onMemberClicked(userId)}
                            onProjectTitleClicked={(projectId) => this.onProjectTitleClicked(projectId)}
                            onCourseTitleClicked={(courseId) => this.onCourseTitleClicked(courseId)}
                        />
                    </div>
                </div>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        email: state.email,
        name: state.name,
        userId: state.userId,
        userType: state.userType,
    };
};

export default connect(mapStateToProps)(Profile);

const dummyUser = {
    information:
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
