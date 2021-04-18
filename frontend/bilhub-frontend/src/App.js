import React, { Component } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';

import { Layout } from './hoc';
// prettier-ignore
import {
    Logout, Login, Signup, Course, CourseAssignment, CourseCreation, CourseSettings,
    Home, Project, ProjectAssignment, Profile, Settings, Notifications
} from './containers';

class App extends Component {
    render() {
        const user_info = JSON.parse(localStorage.getItem('user-info'));
        //const user_token = user_info ? user_info.token : null;
        const user_type = user_info ? user_info.user_type : null;

        const unauthenticatedRoutes = (
            <Switch>
                <Route exact path={'/login'} component={Login} />
                <Route exact path={'/signup'} component={Signup} />
                <Redirect to={'/login'} />
            </Switch>
        );

        const authenticatedRoutes = (
            <Layout>
                <Switch>
                    <Route exact path={'/notifications'} component={Notifications} />
                    <Route exact path={'/project/assignment/:id'} component={ProjectAssignment} />
                    <Route exact path={'/project/:id'} component={Project} />
                    <Route exact path={'/profile'} component={Profile} />
                    <Route exact path={'/profile/:id'} component={Profile} />
                    <Route exact path={'/course/:id'} component={Course} />
                    <Route exact path={'/course/assignment/:id'} component={CourseAssignment} />
                    <Route exact path={'/settings'} component={Settings} />
                    <Route exact path={'/logout'} component={Logout} />
                    {user_type === 'instructor' ? (
                        <>
                            <Route exact path={'/create-new-course'} component={CourseCreation} />
                            <Route exact path={'/course/:id/settings'} component={CourseSettings} />
                        </>
                    ) : null}
                    <Route exact path={'/'} component={Home} />
                    <Redirect to={'/'} />
                </Switch>
            </Layout>
        );

        return true ? authenticatedRoutes : unauthenticatedRoutes;
    }
}

export default App;
