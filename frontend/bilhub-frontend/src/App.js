import React, { Component } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import { connect } from 'react-redux';

import { AppLayout } from './components';
import {
    Login,
    Signup,
    Course,
    CourseAssignment,
    CourseCreation,
    CourseSettings,
    Home,
    Project,
    Profile,
    Settings,
    Notifications,
} from './routes';
import * as actions from './store';

class App extends Component {
    componentDidMount() {
        const token = localStorage.getItem('token');
        this.props.onCheckAuth(token);
    }

    render() {
        if (this.props.appLoading) {
            return <>Loading...</>;
        }

        const unauthenticatedRoutes = (
            <Switch>
                <Route exact path={'/login'} component={Login} />
                <Route exact path={'/signup'} component={Signup} />
                <Redirect to={'/login'} />
            </Switch>
        );

        const authenticatedRoutes = (
            <AppLayout>
                <Switch>
                    <Route exact path={'/notifications'} component={Notifications} />
                    <Route exact path={'/project/:projectId/submission/:submissionPageId'} component={Project} />
                    <Route exact path={'/project/:projectId'} component={Project} />
                    <Route exact path={'/profile'} component={Profile} />
                    <Route exact path={'/profile/:id'} component={Profile} />
                    <Route exact path={'/course/:courseId'} component={Course} />
                    <Route exact path={'/course/:courseId/assignment/:assignmentId'} component={CourseAssignment} />
                    <Route exact path={'/settings'} component={Settings} />
                    {this.props.userType === 'instructor' ? (
                        <Route exact path={'/create-new-course'} component={CourseCreation} />
                    ) : null}
                    {this.props.userType === 'instructor' ? (
                        <Route exact path={'/course/:id/settings'} component={CourseSettings} />
                    ) : null}
                    <Route exact path={'/'} component={Home} />
                    <Redirect to={'/'} />
                </Switch>
            </AppLayout>
        );
        return this.props.token ? authenticatedRoutes : unauthenticatedRoutes;
    }
}

const mapStateToProps = (state) => {
    return {
        userType: state.userType,
        appLoading: state.appLoading,
        token: state.token,
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        onCheckAuth: (token) => dispatch(actions.checkAuth(token)),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
