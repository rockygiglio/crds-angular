import requestChildcareComponent from './requestChildcare.component';
import RequestChildcareService from './requestChildcare.service';
(function() {
  'use strict';
  var MODULE = require('crds-constants').MODULES.MPTOOLS;
  angular.module(MODULE)
    .directive('requestChildcare', requestChildcareComponent)
    .service('RequestChildcareService', RequestChildcareService) 
    ;

  require('./requestChildcare.html');
})();
