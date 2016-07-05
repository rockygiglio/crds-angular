import participantService from './participant.service';
import CONSTANTS from 'crds-constants';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  factory('participant', participantService);
