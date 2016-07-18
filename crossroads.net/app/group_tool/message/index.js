
import messageComponent from './message.component';
import CONSTANTS from 'crds-constants';
import html from './message.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  component('messageComponent', messageComponent())
  ;