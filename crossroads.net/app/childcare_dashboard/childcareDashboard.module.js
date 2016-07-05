import ChildcareDashboard from './childcareDashboard.component';
import ChildcareRoutes from './childcareDashboard.routes';

import constants from '../constants';

angular.module(constants.MODULES.CHILDCARE_DASHBOARD, [])
  .config(ChildcareRoutes)
  .component('childcareDashboard', ChildcareDashboard)
  ;

require('./childcareDashboard.html');
