(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORMLY_BUILDER;
  var ngModule = angular.module(MODULE, ['crossroads.core', 'crossroads.common']);
  
  require('./formlyConfig/types')(ngModule);
  require('./formlyConfig/wrappers')(ngModule);
  require('./formlyWrapper')(ngModule);
  require('./formlyMapper/')(ngModule);
})();
