import React from 'react';
import { IndexRoute, Route } from 'react-router';
import {
    App,
    Home,
    NotFound
} from './containers';

/* eslint-disable */
export default (store) => (
    <Route path="/" component={App}>
        { /* Home (main) route */ }
        <IndexRoute component={Home} />

        { /* Routes */ }

        { /* Catch all route */ }
        <Route path="*" component={NotFound} status={404} />
    </Route>
);
/* eslint-enable */
