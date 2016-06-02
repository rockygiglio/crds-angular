import childcareDecisionComponent from './childcareDecision.component';
import ChildcareDecisionService from './childcareDecision.service';
import CONSTANTS from 'crds-constants';

angular.module(CONSTANTS.MODULES.MPTOOLS)
    .directive('childcareDecision', requestChildcareComponent)
    .service('ChildcareDecisionService', ChildcareDecisionService)
;


require('./childcareDecision.html');
