const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const path = require('path');
const environmentVars = require('./environment.config.js');

const definePlugin = new webpack.DefinePlugin(environmentVars.get());
module.exports = {
  entry: {
    main: './app/app.js',
    misc: './app/misc.js',
    ang: './core/ang.js',
    core: ['./core/core.js'],
    legacy: ['./styles/legacy.scss']
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
