(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.GO_VOLUNTEER;

  require('./goVolunteer.template.html');
  
  angular.module(MODULE, ['crossroads.core', 'crossroads.common'])
    .config(require('./goVolunteer.routes'))
    .factory('GoVolunteerService', require('./goVolunteer.service'))
    ;

  require('./city');
  require('./organizations');
  require('./page');
  

})();
