import CONSTANTS from 'crds-constants';
import streamComponent from './stream.component';
import html from './stream.html';

export default angular.module(CONSTANTS.MODULES.LIVE_STREAM)
  .component('stream', streamComponent());