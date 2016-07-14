(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORMLY_BUILDER;

  angular.module(MODULE, ['crossroads.core', 
                          'crossroads.common'
                          ])
    .config(require('./formlyBuilder.config'));

})();
