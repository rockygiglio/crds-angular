import PlaygroundService from './services/playground.service';
import PlaygroundController from './formlyBuilder_playground.controller';
(function() {
  'use strict';
  
  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.FORMLY_BUILDER_PLAYGROUND, [
    MODULES.COMMON,
    MODULES.CORE
  ])
  .config(require('./formlyBuilder_playground.routes'))
  .service('PlaygroundService', PlaygroundService)
  .controller('PlaygroundController', PlaygroundController)
  ;

})();
