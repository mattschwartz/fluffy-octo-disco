require('babel-polyfill');

const environment = {
    development: {
        isProduction: false
    },
    production: {
        isProduction: true
    }
}[process.env.NODE_ENV || 'development'];

module.exports = Object.assign({
    host: process.env.HOST || 'localhost',
    port: process.env.PORT,
    app: {
        title: 'Pantry',
        description: 'All the best modern practices in one example.',
        head: {
            titleTemplate: '%s ― Pantry',
            meta: [
                { name: 'description', content: 'All the modern best practices in one example.' },
                { charset: 'utf-8' },
            ]
        }
    }
}, environment);
