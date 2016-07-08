import CONSTANTS from 'crds-constants';
import ParticipantService from './services/participant.service';
import groupToolRouter from './groupTool.routes';
import groupToolFormlyBuilderConfig from './groupTool.formlyConfig';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL, [ CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON,
                                          CONSTANTS.MODULES.FORMLY_BUILDER]).
  config(groupToolRouter).
  config(groupToolFormlyBuilderConfig).
  service('Participant', ParticipantService)
  ;

import myGroups from './my_groups'
import createGroup from './create_group'