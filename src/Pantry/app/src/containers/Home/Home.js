import React, { Component } from 'react';
import Helmet from 'react-helmet';

export default
class Home extends Component {

    render() {
        const styles = require('./Home.scss');
        // require the logo image both from client and server
        return (
            <div className={styles.home}>
                <Helmet title="Home" />
                <div className="text-center">
                    <div className="btn-group-vertical">
                        <a className="btn btn-info" href="/account">My Account</a>
                        <a className="btn btn-success" href="/recipes">View Recipes</a>
                        <a className="btn btn-success" href="/recipes/new">Create a new Recipe</a>
                        <a className="btn btn-success" href="/ingredients">View Ingredients</a>
                        <a className="btn btn-success" href="/ingredients/new">Create a new Recipe</a>
                        <a className="btn btn-success" href="/units">View Measurement Units</a>
                        <a className="btn btn-success" href="/units/new">Create a new Measurement Unit</a>
                    </div>
                </div>
            </div>
        );
    }
}
