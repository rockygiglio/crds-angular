(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORMLY_BUILDER;

  angular.module(MODULE, ['crossroads.core', 
                          'crossroads.common', 
                          'formly',
                          'formlyBootstrap'])
    .config(require('./formlyBuilder.config'))
    ;

})();
