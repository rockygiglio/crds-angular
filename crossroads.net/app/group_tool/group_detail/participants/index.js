import groupDetailParticipantsComponent from './groupDetail.participants.component';
import CONSTANTS from 'crds-constants';
import html from './groupDetail.participants.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  component('groupDetailParticipants', groupDetailParticipantsComponent())
  ;