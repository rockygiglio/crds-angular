var webpack = require('webpack');
var path = require('path');

module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine'],
        files: [
            'spec-ts/spec_index.js'
        ],
        exclude: [
        ],
        webpack: {
            resolve: {
                extensions: ['.ts', '.js', '.tsx', '.jsx', '']
            },
            module: {
                loaders: [
                    {
                        test: /\.tsx?$/,
                        exclude: /node_modules/,
                        loader: 'awesome-typescript-loader'
                    },
                    {
                      test: /\.json$/,
                      loaders: ["json-loader"]
                    }
                ]
            }
        },


        // preprocess matching files before serving them to the browser
        // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
        preprocessors: {
            'spec-ts/spec_index.js': ['webpack', 'env', 'sourcemap']
        },

        // test results reporter to use
        // possible values: 'dots', 'progress'
        // available reporters: https://npmjs.org/browse/keyword/karma-reporter
        reporters: ['mocha'],

        // web server port
        port: 9876,

        webpackServer: {
            noInfo: true // prevent console spamming when running in Karma!
        },

        webpackMiddleware: {
            stats: {
                colors: true
            }
        },

        // enable / disable colors in the output (reporters and logs)
        colors: true,

        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        logLevel: config.LOG_INFO,

        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: true,

        // start these browsers
        // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
        browsers: ['PhantomJS'],

        // Continuous Integration mode
        // if true, Karma captures browsers, runs the tests and exits
        singleRun: false,

        // Plugins
        plugins: [
            require('karma-webpack'),
            require('karma-jasmine'),
            require('karma-mocha-reporter'),
            require('karma-phantomjs-launcher'),
            require('karma-env-preprocessor'),
            require('karma-sourcemap-loader'),
            require('karma-es6-shim'),
        ]


    })
}
