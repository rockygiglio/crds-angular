import ChildcareDashboardController from './childcareDashboard.controller';
import ChildcareRoutes from './childcareDashboard.routes';

import constants from '../constants';

angular.module(constants.MODULES.CHILDCARE_DASHBOARD, [])
  .config(ChildcareRoutes)
  .controller('ChildcareDashboardController', ChildcareDashboardController)
  ;

require('./childcareDashboard.html');
