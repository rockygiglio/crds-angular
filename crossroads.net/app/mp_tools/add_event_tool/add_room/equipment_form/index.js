import CONSTANTS from 'crds-constants';
import './equipment_form.html';
import equipmentFormComponent from './equipment_form.component';
import uniqueEqupment from './uniqueEquipment.directive.js';

export default angular
.module(CONSTANTS.MODULES.MPTOOLS)
.component('equipmentFormComponent', equipmentFormComponent())
.directive('uniqueEquipment');//, require('./uniqueEquipment.directive'));

// (function() {
//   'use strict';

//   var MODULE = require('crds-constants').MODULES.MPTOOLS;

//   angular.module(MODULE)
//     .directive('equipmentForm', require('./equipmentForm.component'))
//     .directive('uniqueEquipment', require('./uniqueEquipment.directive'))
//   ;

//   require('./equipmentForm.html');
// })();
