import CONSTANTS from 'crds-constants';

import './myserve.html';
import './one_time_serve_mockup.html';
import './event_registration_mockup.html';
import './event_registration_mockup_desired.html';
import './serveTabs.html';
import './serveTeam.html';
import './refine/refineList.html';
import './refine/serveModalContent.html';

import OpportunityCapacityService from './capacity.service';
import FilterStateService from  './filterState.service.js';

import MyServeController from './myserve.controller';
import ServeModalController from './refine/serveModal.controller';

import ServeTabsDirective from './serveTabs.directive';
import ServeTeamDirective from './serveTeam.directive';
import RefineListDirective from './refine/refineList.directive';

import myServeRouter from './my_serve.routes';

export default angular
  .module(CONSTANTS.MODULES.MY_SERVE, [ CONSTANTS.MODULES.CORE, CONSTANTS.MODULES.COMMON ])

  .config(myServeRouter)

  .factory('ServeTeamFilterState', FilterStateService)
  .factory('OpportunityCapacityService', OpportunityCapacityService)

  .controller('MyServeController', MyServeController)
  .controller('ServeModalController', ServeModalController)

  .directive('serveTabs', ServeTabsDirective)
  .directive('serveTeam', ServeTeamDirective)
  .directive('serveTeamRefineList', RefineListDirective)
;

//
// Module sub-components
//

import './serve_team_message';
