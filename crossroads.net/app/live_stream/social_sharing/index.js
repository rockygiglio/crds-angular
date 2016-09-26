import CONSTANTS from 'crds-constants';
import socialSharingComponent from './socialSharing.component';
import html from './countdown.html';

export default angular.module(CONSTANTS.MODULES.LIVE_STREAM)
  .component('socialSharing', socialSharingComponent());