import PlaygroundService from './services/playground.service';
import PlaygroundController from './formlyBuilder_playground.controller';
import FBPlaygroundRoutes from './formlyBuilder_playground.routes';

var MODULES = require('crds-constants').MODULES;

export default angular
  .module(MODULES.FORMLY_BUILDER_PLAYGROUND, [
  MODULES.COMMON,
  MODULES.CORE
  ])
  .config(FBPlaygroundRoutes)
  .service('PlaygroundService', PlaygroundService)
  .controller('PlaygroundController', PlaygroundController)
  ;