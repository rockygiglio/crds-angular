const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const path = require('path');
const environmentVars = require('./environment.config.js');

const definePlugin = new webpack.DefinePlugin(environmentVars.get());
module.exports = {
  entry: {
    childcare: './app/childcare_dashboard/childcareDashboard.module.js',
    trips: './app/trips/trips.module.js',
    camps: './app/camps/camps.module.js',
    search: './app/search/search.module.js',
    media: './app/media/media.module.js',
    give: './app/give/give.module.js',
    profile: './app/profile/profile.module.js',
    main: './app/app.js',
    ang: './core/ang.js',
    govolunteer: './app/go_volunteer/goVolunteer.module.js',
    core: ['./core/core.js'],
    legacy: ['./styles/legacy.scss'],
    common: ['./app/common/common.module.js'],
    formbuilder: ['./app/formBuilder/formBuilder.module.js'],
    formlybuilder: ['./app/formlyBuilder/formlyBuilder.module.js']
  },
  watchPattern: ['app/**/**', 'core/**/**'],
  externals: {
    stripe: 'Stripe',
    moment: 'moment',
  },
  context: __dirname,
  output: {
    path: './assets',
    publicPath: '/assets/',
    filename: '[name].js',
  },
  resolve: {
    extensions: ['', '.js', '.jsx'],
    alias: {
      constants: './app/constants.js'
    }
  },
  devtool: 'sourcemap',
  debug: true,
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
          path.resolve(__dirname, 'node_modules/angular-stripe')
        ],
        exclude: [
          /streamspotAnalytics\.js$/,
          /videojs5-hlsjs-source-handler/
        ],
        loader: 'ng-annotate!babel-loader'
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
        loader: 'ng-cache?prefix=[dir]',
        exclude: [/\.ng2component\.html$/]
      },
      {
        test: /\.json$/,
        loaders: ['json-loader']
      },
    ],
    noParse: [
      path.join(__dirname, 'node_modules', 'video.js', 'dist', 'video.js'),
      /videojs5-hlsjs-source-handler/,
      path.join(__dirname, 'node_modules', 'videojs-chromecast', 'dist', 'videojs-chromecast.js')
    ]
  },
  plugins: [
    new ExtractTextPlugin('[name].css'),
    definePlugin
  ]
};
