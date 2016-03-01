'use strict';
var MODULE = 'crossroads.go';

angular.module(MODULE, ['crossroads.core', 'crossroads.common'])
  .config(require('./go.routes'))
  ;
require('./signup');
