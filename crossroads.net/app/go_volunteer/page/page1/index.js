(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.GO_VOLUNTEER;

  require('./goVolunteerPage1.template.html');

  angular.module(MODULE)
    .directive('goVolunteerPage1', require('./goVolunteerPage1.component'));


})();
