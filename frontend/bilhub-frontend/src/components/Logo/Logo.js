import React, { Component } from 'react';
import { Image } from 'semantic-ui-react';
import logo from '../../assets/logo.png';
import './Logo.css';
export class Logo extends Component {
    render() {
        return <Image src={logo} className="logo" />;
    }
}
//
//
