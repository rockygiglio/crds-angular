(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORM_BUILDER_V;
  var ngModule = angular.module(MODULE, ['crossroads.core', 'crossroads.common']);
  
  require('./mapper')(ngModule);
  require('./wrapper')(ngModule);
})();
