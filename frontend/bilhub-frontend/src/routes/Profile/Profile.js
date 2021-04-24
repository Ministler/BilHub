import React, { Component } from 'react';
import { Grid, Segment, Tab, Icon } from 'semantic-ui-react';
import './Profile.css';

export class Profile extends Component {
    panes = [
        {
            menuItem: 'Projects',
            render: () => (
                <Tab.Pane as="div" attached={false}>
                    Projects
                </Tab.Pane>
            ),
        },
        {
            menuItem: 'Instructed Courses',
            render: () => (
                <Tab.Pane as="div" attached={false}>
                    Instructed Courses
                </Tab.Pane>
            ),
        },
    ];

    TabExampleSecondaryPointing = () => (
        <Tab menu={{ secondary: true, pointing: true, color: 'red' }} style={{ width: '75%' }} panes={this.panes} />
    );

    render() {
        return (
            <div class="ui centered grid">
                <div class="row">
                    <div class="four wide column">
                        <Segment>
                            <h2 className="ui center aligned icon header">
                                <Icon circular name="user" size="Massive" />
                                Yusuf Uyar
                            </h2>
                            <p align="center">yusuf.uyar@ug.bilkent.edu.tr</p>
                            <h4 style={{ marginLeft: '20px' }}>Bio</h4>
                            <p>
                                Lorem ipsum dolor sit amet consectetur adipisicing elit. Delectus id aspernatur ea sit
                                animi, ab qui! Ea beatae dolorum inventore cum quibusdam placeat quisquam itaque, odio
                                quasi numquam maiores quidem illum odit commodi dicta animi voluptas tempora? Adipisci
                                maiores inventore minus provident quas minima itaque saepe et labore, ut sequi!
                            </p>
                        </Segment>
                    </div>
                    <div class="eight wide column">
                        <this.TabExampleSecondaryPointing></this.TabExampleSecondaryPointing>
                    </div>
                </div>
            </div>
        );
    }
}
