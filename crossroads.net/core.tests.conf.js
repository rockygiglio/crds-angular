var webpack = require('webpack');
var path = require('path');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

var environmentVars = require(path.resolve(__dirname, 'environment.config.js'));
var definePlugin = new webpack.DefinePlugin(environmentVars.get());

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: './',

    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],

    // list of files / patterns to load in the browser
    files: [
      'https://js.stripe.com/v2/',
      './node_modules/phantomjs-polyfill/bind-polyfill.js',
      './node_modules/angular/angular.js',
      'node_modules/angular-mocks/angular-mocks.js',
      'spec-core/spec_index.js'
    ],

    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
      'spec-core/spec_index.js': ['webpack','env']
    },

    envPreprocessor: [
      'CRDS_API_ENDPOINT',
      'CRDS_CMS_ENDPOINT',
    ],

    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['mocha'],

    // web server port
    port: 9876,

    webpack: {
      module: {
        loaders: [
          {
            test: /\.css$/,
            loader: 'style-loader!css-loader'
          },
          {
            test: /\.js$/,
            include: [
              path.resolve(__dirname, 'app'),
              path.resolve(__dirname, 'core'),
              path.resolve(__dirname, './node_modules/angular-stripe')
            ],
            exclude: [
              /streamspotAnalytics\.js$/,
              /videojs5-hlsjs-source-handler/
            ],
            loader: 'babel-loader'
          },
          {
            test: /\.scss$/,
            loader: ExtractTextPlugin.extract('style-loader', 'css-loader!autoprefixer-loader!sass-loader')
          },
          {
            test: /\.(jpe?g|png|gif|svg)$/i,
            loaders: ['image?bypassOnDebug&optimizationLevel=7&interlaced=false']
          },
          {
            test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
            loader: 'url-loader?limit=10000&minetype=application/font-woff'
          },
          {
            test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
            loader: 'file-loader'
          },
          {
            test: /\.html$/,
            loader: 'ng-cache?prefix=[dir]'
          }
        ],
        noParse: [
            path.join(__dirname, "node_modules", "video.js","dist","video.js"),
            /videojs5-hlsjs-source-handler/,
            path.join(__dirname, "node_modules", "videojs-chromecast","dist","videojs-chromecast.js")
        ]
      },
      plugins: [new ExtractTextPlugin('[name].css'), definePlugin]
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
      require('karma-env-preprocessor')
    ]

  });
};
