import React, { Component } from 'react';
import { Segment, Tab } from 'semantic-ui-react';
import './Profile.css';

export class Profile extends Component {
    panes = [
        {
            menuItem: 'Tab 1',
            render: () => <Tab.Pane attached={false}>Tab 1 Content</Tab.Pane>,
        },
        {
            menuItem: 'Tab 2',
            render: () => <Tab.Pane attached={false}>Tab 2 Content</Tab.Pane>,
        },
        {
            menuItem: 'Tab 3',
            render: () => <Tab.Pane attached={false}>Tab 3 Content</Tab.Pane>,
        },
    ];

    TabExampleSecondaryPointing = () => (
        <Tab menu={{ secondary: true, pointing: true, borderRadius: '0px' }} panes={this.panes} />
    );

    render() {
        return (
            <div class="ui centered grid">
                <div class="row">
                    <div class="three wide column">
                        <Segment>
                            <p>
                                Lorem ipsum dolor sit amet consectetur adipisicing elit. Voluptate possimus nulla rem,
                                commodi, cum accusantium sapiente eos fuga maiores iste harum explicabo voluptatum
                                similique? Asperiores repellat hic quidem vero mollitia laborum fugit, nihil inventore
                                aliquam perspiciatis sequi alias corporis autem reiciendis iure, totam provident tenetur
                                at commodi, dolorum rem dicta!
                            </p>
                        </Segment>
                    </div>
                    <div class="eight wide column">
                        <Segment></Segment>
                    </div>
                </div>
            </div>
        );
    }
}
