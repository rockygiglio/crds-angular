(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.GO_VOLUNTEER;

  require('./goVolunteerAvailablePrepSpouse.template.html');

  angular.module(MODULE)
    .directive('goVolunteerAvailablePrepSpouse', require('./goVolunteerAvailablePrepSpouse.component'));
})();
