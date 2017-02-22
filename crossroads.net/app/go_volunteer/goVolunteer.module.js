(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.GO_VOLUNTEER;

  angular.module(MODULE, ['crossroads.core', 'crossroads.common'])
    .config(require('./goVolunteer.routes'))
    .config(require('./goVolunteer.formly'))
    .factory('GoVolunteerService', require('./goVolunteer.service'))
    .factory('Organizations', require('./organizations.service'))
    .factory('GoVolunteerDataService', require('./goVolunteerData.service'))
    .factory('SkillsService', require('./skills.service'))
    .factory('GroupConnectors', require('./groupConnectors.service'))
    .factory('GoVolunteerDataService', require('./goVolunteerData.service'))
    ;

  require('./cms');
  require('./city');
  require('./organizations');
  require('./page');

  require('./anywhereProfile');
  require('./projectCard');
})();
