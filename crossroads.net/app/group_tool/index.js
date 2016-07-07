import CONSTANTS from 'crds-constants';
import ParticipantService from './services/participant.service';
import GroupService from './services/group.service';
import groupToolRouter from './groupTool.routes';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL, [ CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON ]).
  config(groupToolRouter).
  service('Participant', ParticipantService).
  service('Group', GroupService)
  ;

import myGroups from './my_groups';
import createGroup from './create_group';
import groupDetail from './group_detail';
