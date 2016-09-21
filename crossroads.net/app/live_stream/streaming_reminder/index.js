import CONSTANTS from 'crds-constants';
import streamingReminderService from './streamingReminder.service';
import streamingReminderController from './streamingReminder.controller';
import html from './streamingReminder.html';

export default angular.module(CONSTANTS.MODULES.LIVE_STREAM)
  .service('StreamingReminderService', streamingReminderService)
  .controller('StreamingReminderController', streamingReminderController)
;