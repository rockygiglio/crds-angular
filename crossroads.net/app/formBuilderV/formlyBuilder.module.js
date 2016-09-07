(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORMLY_BUILDER;
  var ngModule = angular.module(MODULE, ['crossroads.core', 'crossroads.common']);
  
  ngModule.run(require('./formlyBuilder.validationConfig'));
  
  require('./types')(ngModule);
  require('./wrappers')(ngModule);
})();
