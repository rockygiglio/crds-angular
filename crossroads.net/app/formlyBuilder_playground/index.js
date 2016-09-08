import PlaygroundService from './services/playground.service.js';
(function() {
  'use strict';
  
  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.FORMLY_BUILDER_PLAYGROUND, [
    MODULES.COMMON,
    MODULES.CORE
  ])
  .config(require('./formlyBuilder_playground.routes'))
  .service('PlaygroundService', PlaygroundService)
  ;

})();
