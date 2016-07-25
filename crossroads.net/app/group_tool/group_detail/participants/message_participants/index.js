
import messageParticipantsComponent from './messageParticipants.component';
import CONSTANTS from 'crds-constants';
import html from './messageParticipants.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  component('messageParticipants', messageParticipantsComponent())
  ;