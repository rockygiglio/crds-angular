import requestChildcareComponent from './requestChildcare.component';

(function() {
  'use strict';
  var MODULE = require('crds-constants').MODULES.MPTOOLS;
  angular.module(MODULE).directive('requestChildcare', requestChildcareComponent);

  require('./requestChildcare.html');
})();
