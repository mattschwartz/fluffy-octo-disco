import Express from 'express';
import React from 'react';
import ReactDOM from 'react-dom/server';
import RouterContext from 'react-router/lib/RouterContext';
import config from './config';
import favicon from 'serve-favicon';
import compression from 'compression';
import path from 'path';
import createStore from './redux/create';
import Html from './helpers/Html';
import PrettyError from 'pretty-error';

import { match } from 'react-router';
import { syncHistoryWithStore } from 'react-router-redux';
import createHistory from 'react-router/lib/createMemoryHistory';
import { Provider } from 'react-redux';
import getRoutes from './routes';

const pretty = new PrettyError();
const app = new Express();

// TODO: Figure out how to get this decompressed on the server
// app.use(compression());
app.use(favicon(path.join(__dirname, '..', 'static', 'favicon.ico')));
app.use(Express.static(path.join(__dirname, '..', 'static')));

app.use((req, res) => {
    if (__DEVELOPMENT__) {
        // Do not cache webpack stats: the script file would change since
        // hot module replacement is enabled in the development env
        webpackIsomorphicTools.refresh();
    }

    const data = req.body;

    const memoryHistory = createHistory(req.originalUrl);
    const store = createStore(memoryHistory, data);
    const history = syncHistoryWithStore(memoryHistory, store);

    function hydrateOnClient() {
        res.send('<!doctype html>\n' +
            ReactDOM.renderToString(<Html assets={webpackIsomorphicTools.assets()} store={store} />)); // eslint-disable-line
    }

    match({ history, routes: getRoutes(store), location: req.originalUrl },
        (error, redirectLocation, renderProps) => {
            if (redirectLocation) {
                res.redirect(redirectLocation.pathname + redirectLocation.search);
            } else if (error) {
                console.error('ROUTER ERROR:', pretty.render(error));
                res.status(500);
                hydrateOnClient();
            } else if (renderProps) {
                const component = (
                    <Provider store={store} key="provider">
                        <RouterContext {...renderProps} />
                    </Provider>
                );

                res.status(200);

                global.navigator = {
                    userAgent: req.headers['user-agent']
                };

                res.send('<!doctype html>\n' +
                    ReactDOM.renderToString(<Html assets={webpackIsomorphicTools.assets()} component={component} store={store} />)); // eslint-disable-line
            } else {
                res.status(404).send('Not found');
            }
        });
});

app.listen(config.port, err => {
    if (err) {
        return console.error(err);
    }
    console.info('----\n==> ✅  %s is running.', config.app.title);
});
