(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORMLY_BUILDER;

  angular.module(MODULE, ['crossroads.core', 
                          'crossroads.common',
                          require('angular-formly'),
                          require('angular-formly-templates-bootstrap')])
    .config(require('./formlyBuilder.config'));

})();
