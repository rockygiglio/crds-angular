import CONSTANTS from 'crds-constants';
import groupToolRouter from './groupTool.routes';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL, [ CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON ]).
  config(groupToolRouter);

import myGroups from './my_groups'
