
import removeParticipantComponent from './removeParticipant.component';
import CONSTANTS from 'crds-constants';
import html from './removeComponent.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  component('removeParticipant', removeParticipantComponent())
  ;