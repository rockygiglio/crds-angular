import requestChildcareComponent from './requestChildcare.component';
import RequestChildcareService from './requestChildcare.service';
import endTimeValidation from './sessionTime.validation.directive';
import CONSTANTS from 'crds-constants';

angular.module(CONSTANTS.MODULES.MPTOOLS)
    .directive('requestChildcare', requestChildcareComponent)
    .directive('endTime', endTimeValidation)
    .service('RequestChildcareService', RequestChildcareService)
    ;

require('./requestChildcare.html');
