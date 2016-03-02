(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.GO_VOLUNTEER;

  require('./selectChurch.template.html');

  angular.module(MODULE)
    .directive('selectChurch', require('./selectChurch.component'));


})();
