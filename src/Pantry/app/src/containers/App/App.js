import React, { Component, PropTypes } from 'react';
import Helmet from 'react-helmet';
import config from '../../config';

export default
class App extends Component {

    render() {
        require('./App.scss');

        return (
            <div className={'app'}>
                <Helmet {...config.app.head} />
                <p>hi mom</p>
                <div className={'appContent'}>
                    {this.props.children}
                </div>
            </div>
        );
    }
}

App.propTypes = {
    children: PropTypes.object.isRequired
};
